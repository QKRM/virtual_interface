변수선언   
```cs
    public Camera cam;
    public Material defaultMaterial;

    private int positionCount = 2;
    private LineRenderer curLine;

    private Vector3 PrevPos = Vector3.zero;
```
화면을 터치했을때 그 위치를 Unity 3dworld의 위치로 바꿔줌.   
0.3f는 깊이감을 주기위해 (화면은 2d, 월드는 3d)   
터치가 눌리면 createLine 호출, 떨어지면 connectLine호출   
```cs
    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.3f));

        if (Input.GetMouseButtonDown(0))
        {
            createLine(mousePos);
        }else if (Input.GetMouseButton(0))
        {
            connectLine(mousePos);
        }
    }

```

정확히는 LineRender를 설정   
기본 컴퍼넌트들과 mousePos를 대입해줌   

```cs
    void createLine(Vector3 mousePos)
    {
        positionCount = 2;
        GameObject line = new GameObject("Line");
        LineRenderer lineRend = line.AddComponent<LineRenderer>();

        line.transform.parent = cam.transform;
        line.transform.position = mousePos;

        lineRend.startWidth = 0.01f;
        lineRend.endWidth = 0.01f;
        lineRend.numCornerVertices = 5;
        lineRend.numCapVertices = 5;
        lineRend.material = defaultMaterial;
        lineRend.SetPosition(0, mousePos);
        lineRend.SetPosition(1, mousePos);

        curLine = lineRend;
    }
```

이전 위치와 거리차이가 나면 Linerender를 활용해 Line을 그림
```cs
    void connectLine(Vector3 mousePos)
    {
        if (PrevPos != null && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.001f)
        {
            PrevPos = mousePos;
            positionCount++;
            curLine.positionCount = positionCount;
            curLine.SetPosition(positionCount - 1, mousePos);
        }
    }
```