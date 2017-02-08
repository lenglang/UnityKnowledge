using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 使用说明：直接添加脚本到物体上即可，然后设置物体旋转的方向，物体旋转的轴
/// </summary>
public class RotateGestures : MonoBehaviour
{
    float angle = 0;
    Vector2 startPos;
    Vector2 targetPos;
    Vector2 dragPos;
    Vector2 middlePos;
    Vector2 currentDragDir;
    Vector2 previousDragDir;
    Vector3 startDragAngle;
    /// <summary>
    /// 0顺逆,1为顺时针 -1为逆时针
    /// </summary>
    [MyRange(0,2,"旋转方向（0顺逆,1顺时针，2逆时针）")]
    public int direction =1;
    /// <summary>
    /// 0为沿着x轴旋转 1为沿着y轴旋转 2为沿着z轴旋转
    /// </summary>
    [MyRange(0,2,"旋转轴（0x轴，1y轴，2z轴）")]
    public int shaft = 2;
    public Action onBeginDrag;
    public Action onEndDrag;
    /// <summary>
    /// 定义角度委托变量
    /// </summary>
    public Action<float> onRotateAngle;
    void Start()
    {
        //场景中需要一个Tag为MainCamera摄像机
        targetPos = Camera.main.WorldToScreenPoint(this.transform.position);
    }
    void OnEnable()
    {
        EventTriggerListener.Get(gameObject).onBeginDrag = OnBeginDrag;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onEndDrag = OnEndDrag;
    }
    void OnDisable()
    {
        OnDestroy();
    }
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnBeginDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        startPos = evenData.position;
        currentDragDir = (startPos - targetPos).normalized;
        previousDragDir = currentDragDir;
        startDragAngle = this.transform.localEulerAngles;
		angle = 0;
        if (onBeginDrag != null) onBeginDrag();
    }
    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    public void OnDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        dragPos = evenData.position;
        previousDragDir = currentDragDir;
        currentDragDir = (dragPos - targetPos).normalized;
        int rotateDir = RotationDirection(currentDragDir.normalized, previousDragDir.normalized);
        if (rotateDir == direction||direction==0)
        {
            angle += Vector2.Angle(currentDragDir, previousDragDir);
            Vector3 currentHandleBarAngles=Vector3.one;
            switch (shaft)
            {
                case 0:
                    currentHandleBarAngles = new Vector3(startDragAngle.x + angle * rotateDir, startDragAngle.y, startDragAngle.z);
                    break;
                case 1:
                    currentHandleBarAngles = new Vector3(startDragAngle.x, startDragAngle.y + angle * rotateDir, startDragAngle.z);
                    break;
                case 2:
                    currentHandleBarAngles = new Vector3(startDragAngle.x, startDragAngle.y, startDragAngle.z - angle * rotateDir);
                    break;
                default:
                    break;

            }
            this.transform.localEulerAngles = currentHandleBarAngles;
            if (onRotateAngle != null) onRotateAngle(angle);
        }
    }
    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnEndDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject obj)
    {
        if (onEndDrag != null) onEndDrag();
    }
    /// <summary>
    /// 判断旋转顺逆时针
    /// </summary>
    /// <param name="currentDir"></param>
    /// <param name="previousDir"></param>
    /// <returns></returns>
    public static int RotationDirection(Vector3 currentDir, Vector3 previousDir)
    {
        if (Vector3.Cross(currentDir, previousDir).z > 0)
        {
            //顺时针
            return 1;
        }
        else
        {
            //逆时针
            return -1;
        }
    }
    void OnDestroy()
    {
        EventTriggerListener etl=gameObject.GetComponent<EventTriggerListener>();
        if ( etl== null) return;
        EventTriggerListener.Get(gameObject).onBeginDrag = null;
        EventTriggerListener.Get(gameObject).onDrag = null;
        EventTriggerListener.Get(gameObject).onEndDrag = null;
        onRotateAngle = null;
        onEndDrag = null;
		onBeginDrag = null;
        Destroy(etl);
    }
}
