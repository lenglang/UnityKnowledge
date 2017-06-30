using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 0.5f;
    public float moveSpeed = 0.5f;
    public float MaxZoom = 5f;
    public float Minoom = -5f;
    public float MaxX = 5f;
    public float MinX = -5f;
    public float MaxY = 5f;
    public float MinY = -5f;

    float scrollZoomSpeed = 10;
    Vector2 lastTouchPos1 = Vector2.zero;
    Vector2 lastTouchPos2 = Vector2.zero;
    bool startPosFlag;
    float distance = 0f;
    float distanceX = 0f;
    float distanceY = 0f;

    Vector3 reboundX = Vector3.zero;
    Vector3 reboundY = Vector3.zero;
    Vector3 reboundZoom = Vector3.zero;

    bool isCD;

    public float Distance
    {
        get
        {
            return distance;
        }

        set
        {
            distance = value;
        }
    }

    void Start()
    {
        Debug.Log(isCD);
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        //if (DataManager.instance.IsCatchUpCat || DataManager.instance.IsDragFood || isCD) return;

#if UNITY_EDITOR
        //这里是鼠标控制
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var forward = scrollZoomSpeed * Input.GetAxis("Mouse ScrollWheel");
            ZoomPosition(forward);
        }

        if (Input.GetMouseButton(0))
        {
            MovePostion();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReboundPosition();
        }
#endif

        //这里是在手机上手势控制
        if (Input.touchCount > 1)
        {
            //DataManager.instance.IsDragMap = true;
            startPosFlag = false;
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector2 v1 = Input.GetTouch(0).position;
                Vector2 v2 = Input.GetTouch(1).position;
                float distance = GetDistance(v1, v2);
                var forward = distance * Time.deltaTime * zoomSpeed;
                ZoomPosition(forward);
                lastTouchPos1 = Input.GetTouch(0).position;
                lastTouchPos2 = Input.GetTouch(1).position;
            }
            else
            {
                lastTouchPos1 = Vector2.zero;
                lastTouchPos2 = Vector2.zero;
            }

            if (Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ReboundPosition();
            }
        }
        else if (Input.touchCount == 1)
        {  //触摸类型为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPosFlag = true;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved && startPosFlag)
            { //根据触摸点计算X与Y位置   
                MovePostion();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                startPosFlag = false;
                ReboundPosition();
            }
        }
    }


    private float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        if (lastTouchPos1 == Vector2.zero && lastTouchPos2 == Vector2.zero)
        {
            return 0.0f;
        }

        return (pos2 - pos1).sqrMagnitude - (lastTouchPos2 - lastTouchPos1).sqrMagnitude;
    }

    void MovePostion()
    {
        float x = Input.GetAxis("Mouse X") * -1; //Input 获得左右 左负数 右整数 最大1 最小-1
        float z = Input.GetAxis("Mouse Y") * -1;   //获得前后 和Horizontal同个道理

#if UNITY_EDITOR
        moveSpeed = 5f;
#endif

        float MoveX = x * Time.deltaTime * moveSpeed;
        float MoveY = z * Time.deltaTime * moveSpeed;

        if (Mathf.Abs(MoveX) > Mathf.Abs(MoveY))
        {
            if (MoveX == 0) return;
            if (MoveX > 0)
            {
                if (distanceX >= MaxX)
                {
                    if (reboundX.Equals(Vector3.zero))
                    {
                        reboundX = mainCamera.transform.position;
                    }
                }
            }
            else
            {
                if (distanceX <= MinX)
                {
                    if (reboundX.Equals(Vector3.zero))
                    {
                        reboundX = mainCamera.transform.position;
                    }
                }
            }

            distanceX += MoveX;
            mainCamera.transform.Translate(new Vector3(MoveX, 0, 0)); //给Camera的transform 平移不断增量加位置
            //DataManager.instance.IsDragMap = true;
        }
        else
        {
            if (MoveY == 0) return;
            if (MoveY > 0)
            {
                if (distanceY >= MaxY)
                {
                    if (reboundY.Equals(Vector3.zero))
                    {
                        reboundY = mainCamera.transform.position;
                    }
                }
            }
            else
            {
                if (distanceY <= MinY)
                {
                    if (reboundY.Equals(Vector3.zero))
                    {
                        reboundY = mainCamera.transform.position;
                    }
                }
            }

            distanceY += MoveY;
            mainCamera.transform.Translate(new Vector3(0, MoveY, 0)); //给Camera的transform 平移不断增量加位置
            //DataManager.instance.IsDragMap = true;
        }
    }

    void ReboundPosition()
    {
        //DataManager.instance.IsDragMap = false;
        if (!reboundX.Equals(Vector3.zero) || !reboundY.Equals(Vector3.zero))
        {
            var springback = mainCamera.transform.position;
            if (!reboundY.Equals(Vector3.zero))
            {
                var endY = Mathf.Clamp(distanceY, MinY, MaxY);
                var tmpY = mainCamera.transform.up * (distanceY - endY);
                distanceY = endY;
                springback -= tmpY;
            }

            if (!reboundX.Equals(Vector3.zero))
            {
                var endX = Mathf.Clamp(distanceX, MinX, MaxX);
                var tmpX = mainCamera.transform.right * (distanceX - endX);
                distanceX = endX;
                springback -= tmpX;
            }

            if (reboundX.Equals(Vector3.zero) && reboundY.Equals(Vector3.zero))
            {
                return;
            }

            isCD = true;
            //CatGameTools.UIGameObjectPositionBack(mainCamera.transform, springback, 0.5f, () =>
            //{
            //    isCD = false;
            //    reboundX = Vector3.zero;
            //    reboundY = Vector3.zero;
            //});
        }

        if (!reboundZoom.Equals(Vector3.zero))
        {
            if (Distance >= MaxZoom)
            {
                Distance = MaxZoom;
            }
            else if (Distance <= Minoom)
            {
                Distance = Minoom;
            }
            else
            {
                reboundZoom = Vector3.zero;
            }

            if (reboundZoom.Equals(Vector3.zero))
            {
                return;
            }

            isCD = true;
            //CatGameTools.UIGameObjectPositionBack(mainCamera.transform, reboundZoom, 0.5f, () =>
            //{
            //    isCD = false;
            //    reboundZoom = Vector3.zero;
            //});
        }
    }

    void ZoomPosition(float forward)
    {
        if (forward == 0) return;
        if (forward > 1)
            forward = 1;
        else if (forward < -1)
            forward = -1;

        if (forward > 0)
        {
            if (Distance >= MaxZoom)
            {
#if UNITY_EDITOR
                return;
#else
                    if (reboundZoom.Equals(Vector3.zero))
                    {
                        reboundZoom = mainCamera.transform.position;
                    }
#endif
            }

            if (Distance > MaxZoom * 2f)
            {
                return; //最最大了
            }
        }
        else
        {
            if (Distance <= Minoom)
            {
#if UNITY_EDITOR
                return;
#else
                    if (reboundZoom.Equals(Vector3.zero))
                    {
                        reboundZoom = mainCamera.transform.position;
                    }
#endif
            }
        }

        Distance += forward;
        mainCamera.transform.position = mainCamera.transform.position + forward * mainCamera.transform.forward;
    }
}