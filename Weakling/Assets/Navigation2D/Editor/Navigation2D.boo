# Navigation2D Script (c) noobtuts.com 2015
import UnityEditor
import UnityEngine

class Navigation2D(EditorWindow):
    # options
    groundScale as single = 1
    static showNavMesh = 1 # 0 = don't show, 1 = wireframe, 2 = full
    
    # Helper Functions #########################################################    
    def adjust_minmax(co as Collider, min as Vector2, max as Vector2):
        min.x = Mathf.Min(co.bounds.min.x, min.x)
        min.y = Mathf.Min(co.bounds.min.z, min.y)
        max.x = Mathf.Max(co.bounds.max.x, max.x)
        max.y = Mathf.Max(co.bounds.max.z, max.y)
        return min, max
    
    def is_valid_collider(co as Collider2D):
        # usable for navmesh generation if not trigger and if navigation static
        navstatic = GameObjectUtility.AreStaticEditorFlagsSet(co.gameObject, StaticEditorFlags.NavigationStatic)
        return navstatic and co.enabled and not co.isTrigger
    
    def rotation_to_3d(v as Vector3):
        return Vector3(0, -v.z, 0)

    def scale_to_3d(v as Vector3):
        return Vector3(v.x, 1, v.y)
    
    def scale_from_boxcollider2d(co as BoxCollider2D):
        # transform.localScale * collider size (but with components swapped for 3d)
        return Vector3.Scale(scale_to_3d(co.transform.localScale), Vector3(co.size.x, 1, co.size.y))
    
    def scale_from_circlecollider2d(co as CircleCollider2D):
        # transform.localScale * collider size (but with components swapped for 3d)
        return Vector3.Scale(scale_to_3d(co.transform.localScale), Vector3(co.radius, 1, co.radius))
    
    # add colliders generic
    def add_collider2ds(colliders as (Collider2D), primitiveType as int, scalefn as ICallable, parent as Transform):
        # find all valid colliders, add them to projection
        bcs = [co for co in colliders if is_valid_collider(co)]
        for b as Collider2D in bcs:
            # note: creating a primitive is necessary in order for it to bake properly
            g = GameObject.CreatePrimitive(primitiveType)
            g.isStatic = true
            g.transform.parent = parent            
            # position via offset and transformpoint
            localPos = Vector3(b.offset.x, b.offset.y, 0)
            worldPos = b.transform.TransformPoint(localPos)
            g.transform.position = Vector3(worldPos.x, 0, worldPos.y)
            # scale depending on scale * collider size (circle=radius/box=size/...)
            g.transform.localScale = scalefn(b)
            # rotation
            g.transform.rotation = Quaternion.Euler(rotation_to_3d(b.transform.eulerAngles))
            
            # fix a bug where Unity's Navigation system would also add a walkable area
            # on top of a collider that is big enough (which happens often)
            # -> we simply duplicate it very often and decrease the xz scale
            #    and increase the y scale all the time (in other words: like
            #    a pyramid)
            last = g
            for i in range(100): # 50 is enough usually
                gcopy = Instantiate(last)
                gcopy.transform.parent = parent
                scale = gcopy.transform.localScale
                scale.x *= 0.9 # relative so it never becomes negative etc.
                scale.y += 10  # so that it's FAR out of stepheight
                scale.z *= 0.9 # relative so it never becomes negative etc.
                gcopy.transform.localScale = scale
                last = gcopy
                
    def bake_navmesh2d():
        # create a temporary parent GameObject
        obj = GameObject()                
        
        # find all static box colliders, add them to projection
        add_collider2ds(GameObject.FindObjectsOfType[of BoxCollider2D](),
                        PrimitiveType.Cube,
                        scale_from_boxcollider2d,
                        obj.transform)
        # find all static circle colliders, add them to projection
        add_collider2ds(GameObject.FindObjectsOfType[of CircleCollider2D](),
                        PrimitiveType.Cylinder,
                        scale_from_circlecollider2d,
                        obj.transform)
        
        # min and max point needed for ground plane (from 3d colliders)
        cols = GameObject.FindObjectsOfType[of Collider]()
        if cols.Length > 0:
            min = Vector2(Mathf.Infinity, Mathf.Infinity)
            max = -min
            for c in cols:
                min, max = adjust_minmax(c, min, max)
                            
            # create ground (cube instead of plane because it has unit size)
            # (pos between min and max; scaled to fit min and max * scale)
            # note: scale.y=0 so that *groundScale doesn't make it too high
            g = GameObject.CreatePrimitive(PrimitiveType.Cube)
            g.isStatic = true
            g.transform.parent = obj.transform
            w = max.x - min.x
            h = max.y - min.y
            g.transform.position = Vector3(min.x + w/2, -0.5, min.y + h/2)
            g.transform.localScale = Vector3(w, 0, h) * groundScale
        
        # bake navmesh asynchronously, clear mesh
        NavMeshBuilder.BuildNavMeshAsync() # Async causes weird results
        if gizmesh:
            gizmesh.Clear()
        needs_rebuild = true # rebuild as soon as async baking is finished
        
        # delete the gameobjects now that the path was created
        GameObject.DestroyImmediate(obj)
    
    # Editor Window ############################################################
    [MenuItem("Window/Navigation2D")]
    public static def ShowWindow():
        # Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Navigation2D))
        
    def OnGUI():
        GUILayout.BeginVertical()
        
        # instructions
        GUILayout.Label("Navigation2D by noobtuts.com\n")
        GUILayout.Label("Instructions:", EditorStyles.boldLabel)
        GUILayout.Label(" 1. Make Box/Circle-Collider2Ds Static")
        GUILayout.Label(" 2. Press Bake and wait until it's done")
        GUILayout.Label(" 3. Add NavMeshAgent2D to agents\n")
        GUILayout.Label("Notes:", EditorStyles.boldLabel)
        GUILayout.Label(" - Modify Window->Navigation->Agent Radius for path width")
        GUILayout.Label(" - Use GroundScale to cover the outside of your level\n")
        
        # options
        groundScale = EditorGUILayout.Slider("Ground Scale", groundScale, 1, 100)
        showNavMesh = EditorGUILayout.IntPopup("Show Navmesh", showNavMesh, ("Hide", "Wireframe", "Full"), (0, 1, 2));
        
        # repaint scene if showNavMesh option changed
        if GUI.changed:
            SceneView.RepaintAll()
        
        # buttons
        GUILayout.BeginHorizontal()
        if GUILayout.Button("Clear"):
            NavMeshBuilder.ClearAllNavMeshes()
            if gizmesh:
                gizmesh.Clear()
        if NavMeshBuilder.isRunning:
            if GUILayout.Button("Cancel"):
                NavMeshBuilder.Cancel()
        else:                
            if GUILayout.Button("Bake"):
                bake_navmesh2d()         
        GUILayout.EndHorizontal()
        
        GUILayout.EndVertical()
        
    # Gizmo ####################################################################
    static needs_rebuild = false
    static gizmesh as Mesh
    static def rebuild_gizmesh(nm as NavMeshTriangulation):
        if not gizmesh: # is cleared after stopping the game
            gizmesh = Mesh()
        gizmesh.vertices = array(Vector3(v.x, v.z, 0) for v in nm.vertices) # 2D
        gizmesh.triangles = nm.indices
        gizmesh.normals = array(Vector3(0, 0, -1) for _ in gizmesh.vertices)
        needs_rebuild = false
    
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static def OnGizmo(tf as Transform, gt as GizmoType):
        # rebuild if necessary
        if not gizmesh or needs_rebuild:
            if not NavMeshBuilder.isRunning:
                rebuild_gizmesh(NavMesh.CalculateTriangulation())
        
        # draw if not empty
        if gizmesh.vertices.Length:
            Gizmos.color = Color.cyan
            if showNavMesh == 1:
                Gizmos.DrawWireMesh(gizmesh)            
            if showNavMesh == 2:
                Gizmos.DrawMesh(gizmesh)
                Gizmos.DrawWireMesh(gizmesh)
    