변수선언   
'''cs   
    public GameObject obj;

    ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static List<ARRaycastHit> centerHits = new List<ARRaycastHit>();

    private Vector2 touchPos;

    bool isSpawned;
    private GameObject spawnedObj;

    public Camera ARCam;
    public GameObject ARIndicator;
'''   
