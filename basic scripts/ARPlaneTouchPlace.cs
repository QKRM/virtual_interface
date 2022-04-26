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

    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        var screenCenter = ARCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if(arRaycastManager.Raycast(screenCenter, centerHits, TrackableType.All))
        {
            Pose hitPos = centerHits[0].pose;

            var cameraForward = ARCam.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            hitPos.rotation = Quaternion.LookRotation(cameraBearing);
            ARIndicator.SetActive(true);
            ARIndicator.transform.SetPositionAndRotation(hitPos.position, hitPos.rotation);
        }

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
    }
}
