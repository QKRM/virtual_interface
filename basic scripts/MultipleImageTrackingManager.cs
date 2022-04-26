using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImageTrackingManager : MonoBehaviour
{
    public GameObject[] objs;
    private Dictionary<string, GameObject> spawnedObjs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager TrackedImageManager;

    private void Awake()
    {
        TrackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach(GameObject prefab in objs)
        {
            GameObject clone = Instantiate(prefab);
            spawnedObjs.Add(prefab.name, clone);
            clone.SetActive(false);
        }
        //첫번째 루프 prefab은 objs[0]
        //두번째 루프 prefab은 objs[1]
        //세번째 루프 prefab은 objs[2]...
    }

    private void OnEnable()
    {
        TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach(var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach(var trackedImage in eventArgs.removed)
        {
            spawnedObjs[trackedImage.name].SetActive(false);
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        //string name = trackedImage.referenceImage.name;
        GameObject trackedObject = spawnedObjs[trackedImage.referenceImage.name];

        if(trackedImage.trackingState == TrackingState.Tracking)
        {
            trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.rotation = trackedImage.transform.rotation;
            trackedObject.SetActive(true);
        }
        else
        {
            trackedObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
