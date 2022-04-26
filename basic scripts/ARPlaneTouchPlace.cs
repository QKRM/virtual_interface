using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneTouchPlace : MonoBehaviour
{
    public GameObject obj;

    ARRaycastManager arRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static List<ARRaycastHit> centerHits = new List<ARRaycastHit>();

    private Vector2 touchPos;

    bool isSpawned;
    private GameObject spawnedObj;

    public Camera ARCam;
    public GameObject ARIndicator;

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var screenCenter = ARCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if(arRaycastManager.Raycast(screenCenter, centerHits, TrackableType.All)) //indicator 생성
        {
            Pose hitPos = centerHits[0].pose;

            var cameraForward = ARCam.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            hitPos.rotation = Quaternion.LookRotation(cameraBearing);
            ARIndicator.SetActive(true);
            ARIndicator.transform.SetPositionAndRotation(hitPos.position, hitPos.rotation);
        }

        if(Input.touchCount > 0) // 터치되면 물체 생성
        {
            touchPos = Input.GetTouch(0).position;
            if(arRaycastManager.Raycast(touchPos, hits,TrackableType.PlaneWithinPolygon)) //평면이 부딪혔으면
            {
                var hitPos = hits[0].pose;

                if (!isSpawned) //안돼있으면 생성
                {
                    spawnedObj = Instantiate(obj, hitPos.position, hitPos.rotation);
                    isSpawned = true;
                }
                else // 돼있으면 위치 바꾸기
                {
                    spawnedObj.transform.position = hitPos.position;
                }
            }
        }
    }
}
