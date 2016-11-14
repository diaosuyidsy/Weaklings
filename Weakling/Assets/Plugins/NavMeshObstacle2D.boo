# Navigation2D Script (c) noobtuts.com 2015
import UnityEngine

class NavMeshObstacle2D(MonoBehaviour):
    
    # navmeshobstacle properties
    public shape = NavMeshObstacleShape.Box
    public center = Vector2.zero
    public size = Vector2.one
    
    # carving is experimental because it's really hard to debug in 2D where we
    # can't see the navmesh in scene while playing (yet)
    public carve = false
    
    # the projection
    obst as NavMeshObstacle
    
    # project agent position to 2D
    def project_to_2d(v as Vector3):
        return Vector2(v.x, v.z)
    
    # project 2D position to obstacle position
    def project_to_3d(v as Vector2):
        return Vector3(v.x, 0, v.y)
    
    def rotation_to_3d(v as Vector3):
        return Vector3(0, v.z, 0)
    
    def Awake():
        # create projection
        g = GameObject.CreatePrimitive(PrimitiveType.Cylinder)
        g.name = "PATH2D_OBSTACLE"
        g.transform.position = project_to_3d(transform.position)
        g.transform.rotation = Quaternion.Euler(rotation_to_3d(transform.eulerAngles))
        obst = g.AddComponent[of NavMeshObstacle]()
        # disable navmesh and collider (no collider for now...)
        Destroy(obst.GetComponent[of Collider]())
        Destroy(obst.GetComponent[of MeshRenderer]())
    
    def Update():
        # copy properties to projection all the time
        # (in case they are modified after creating it)
        obst.carving = carve
        obst.center = project_to_3d(center)
        obst.size = Vector3(size.x, 1, size.y)
        
        # scale and rotate to match scaled/rotated sprites center properly
        obst.transform.localScale = Vector3(transform.localScale.x, 1, transform.localScale.y)
        obst.transform.rotation = Quaternion.Euler(rotation_to_3d(transform.eulerAngles))
        
        # project position to 3d
        obst.transform.position = project_to_3d(transform.position)
        
    def OnDestroy():
        # destroy projection if not destroyed yet
        if obst:
            Destroy(obst.gameObject)
    
    def OnEnable():
        if obst:
            obst.enabled = true
        
    def OnDisable():
        if obst:
            obst.enabled = false
    
    # Radius Gizmo #############################################################
    # (gizmos.matrix for correct rotation)
    def OnDrawGizmosSelected():
        Gizmos.color = Color.green
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.localRotation, transform.localScale)
        Gizmos.DrawWireCube(Vector3.zero, size) 
    
    # NavMeshObstacle proxy functions ##########################################
    public velocity as Vector2:
        get:
            return project_to_2d(obst.velocity)
        # set: is a bad idea

    # validation
    def OnValidate():
        # force shape to box for now because we would need a separate Editor GUI
        # script to switch between size and radius otherwise
        shape = NavMeshObstacleShape.Box
    