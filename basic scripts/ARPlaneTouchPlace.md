변수선언   
```cs   
    public GameObject obj;

    ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static List<ARRaycastHit> centerHits = new List<ARRaycastHit>();

    private Vector2 touchPos;

    bool isSpawned;
    private GameObject spawnedObj;

    public Camera ARCam;
    public GameObject ARIndicator;

```

화면의 가운데를 ARcamera의 가운데와 맞춤
```cs
var screenCenter = ARCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
```

arRaycastManager의 Raycast기능을 사용해 객체들을 추적함   
ARIndicator를 Raycast된 객체의 위치에 활성화 하고, 부딪힌 곳과 평행하게 rotation시킴
```cs
    if(arRaycastManager.Raycast(screenCenter, centerHits, TrackableType.All))
    {
        Pose hitPos = centerHits[0].pose;

        var cameraForward = ARCam.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

        hitPos.rotation = Quaternion.LookRotation(cameraBearing);
        ARIndicator.SetActive(true);
        ARIndicator.transform.SetPositionAndRotation(hitPos.position, hitPos.rotation);
    }
```

터치가 됬다면 터치된 위치에 PlaneWithinPolygon이 있는지 확인.
있고 물체가 spawn되있지 않다면 물체를 spawn
spawn되있다면 물체 위치를 바꿈
```cs
    if(Input.touchCount > 0)
    {
        touchPos = Input.GetTouch(0).position;
        if(arRaycastManager.Raycast(touchPos, hits,TrackableType.PlaneWithinPolygon))
        {
            var hitPos = hits[0].pose;

            if (!isSpawned)
            {
                spawnedObj = Instantiate(obj, hitPos.position, hitPos.rotation);
                isSpawned = true;
            }
            else
            {
                spawnedObj.transform.position = hitPos.position;
            }
        }
    }
```