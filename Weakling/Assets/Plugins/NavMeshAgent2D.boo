# Navigation2D Script (c) noobtuts.com 2015
import UnityEngine

class NavMeshAgent2D(MonoBehaviour):
    # navmeshagent properties
    public radius as single = 0.5
    public speed as single = 3.5
    public angularSpeed as single = 120
    public acceleration as single = 8
    public stoppingDistance as single = 0
    public autoBraking as bool = false
    
    # the projection
    agent as NavMeshAgent
    
    # project agent position to 2D
    def project_to_2d(v as Vector3):
        return Vector2(v.x, v.z)
    
    # project 2D position to agent position
    def project_to_3d(v as Vector2):
        return Vector3(v.x, 0, v.y)
        
    def Awake():
        # create projection
        g = GameObject.CreatePrimitive(PrimitiveType.Cylinder)
        g.name = "PATH2D_AGENT"
        g.transform.position = project_to_3d(transform.position) # todo height 0.5 again?
        agent = g.AddComponent[of NavMeshAgent]()
        # disable navmesh and collider (no collider for now...)
        Destroy(agent.GetComponent[of Collider]())
        Destroy(agent.GetComponent[of MeshRenderer]())
    
    def FixedUpdate():
        # copy properties to projection all the time
        # (in case they are modified after creating it)
        agent.radius = radius
        agent.speed = speed
        agent.angularSpeed = angularSpeed
        agent.acceleration = acceleration
        agent.stoppingDistance = stoppingDistance
        agent.autoBraking = autoBraking
                
        # copy projection's position
        rb = GetComponent[of Rigidbody2D]()
        if rb and not rb.isKinematic:
            rb.MovePosition(project_to_2d(agent.transform.position))
        else:
            transform.position = project_to_2d(agent.transform.position)
        
        # got stuck? (if distance to projection > collider width * 2)
        dist = Vector2.Distance(transform.position, project_to_2d(agent.transform.position))
        bounds = GetComponent[of Collider2D]().bounds
        if dist > Mathf.Max(bounds.extents.x, bounds.extents.y) * 2:
            # stop agent movement, reset it to current position
            agent.ResetPath()
            agent.transform.position = project_to_3d(transform.position)
            Debug.Log("stopped agent because of collision in 2D plane")
        
    def OnDestroy():
        # destroy projection if not destroyed yet
        if agent:
            Destroy(agent.gameObject)
    
    def OnEnable():
        if agent:
            agent.enabled = true
        
    def OnDisable():
        if agent:
            agent.enabled = false
    
    # Radius Gizmo #############################################################
    # (gizmos.matrix for correct rotation)
    def OnDrawGizmosSelected():
        Gizmos.color = Color.green
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.localRotation, transform.localScale)
        Gizmos.DrawWireSphere(Vector3.zero, radius)
    
    # NavMeshAgent proxy functions #############################################
    # .destination property that forwards it to the agent
    public destination as Vector2:
        get:
            return project_to_2d(agent.destination)
        set:
            agent.destination = project_to_3d(value)
    
    public def SetDestination(v as Vector2):
        destination = v
    
    # ResetPath function to stop
    public def ResetPath():
        agent.ResetPath()
        
    public velocity as Vector2:
        get:
            return project_to_2d(agent.velocity)
        # set: is a bad idea
    
    # Stop
    public def Stop():
        agent.Stop()
    
    # Resume
    public def Resume():
        agent.Resume()
