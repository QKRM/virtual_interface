Awake() 게임이 시작되고 호출   
OnEnable() 활성화 될때마다   
OnDisable() 비활성화 될때마다   


이미지가 트래킹되면, 이미지의 transform에 맞추어 object를 생성   
```cs
    void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
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
```
게임이 시작되면, spawn할 obj들을 Instantiate하고 비활성화 시킴
```cs
    private void Awake()
    {
        TrackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach(GameObject prefab in objs)
        {
            GameObject clone = Instantiate(prefab);
            spawnedObjs.Add(prefab.name, clone);
            clone.SetActive(false);
        }
    }
```

이벤트가 발생하면 경우에 따라 UpdateImage를 호출하거나, 소환된 obj를 비활성화   
```cs
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
```
