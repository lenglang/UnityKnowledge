using UnityEngine;
using WZK;

public class JoystackCc : MonoBehaviour
{
    private Vector3 Origin;
    Transform mTrans;
    private Vector3 _deltaPos;
    private bool _drag = false;

    private Vector3 deltaPosition;

    float dis;
    [SerializeField]
    private float MoveMaxDistance = 80; //最大拖动距离

    [HideInInspector]
    public Vector3 FiexdMovePosiNorm; //固定8个角度移动的距离

    [HideInInspector]
    public Vector3 MovePosiNorm;  //标准化移动的距离
    [SerializeField]
    private float ActiveMoveDistance = 1;               //激活移动的最低距离
    void Awake()
    {
        EventTriggerListener.Get(gameObject)._onDrag = OnDrag;
        EventTriggerListener.Get(gameObject)._onEndDrag = OnDragOut;

        EventTriggerListener.Get(gameObject)._onDown = OnMoveStart;
    }
    // Use this for initialization
    void Start()
    {
        Origin = transform.localPosition; //设置原点
        mTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        dis = Vector3.Distance(transform.localPosition, Origin); //拖动距离，这不是最大的拖动距离，是根据触摸位置算出来的
        if (dis >= MoveMaxDistance)       //如果大于可拖动的最大距离
        {
            Vector3 vec = Origin + (transform.localPosition - Origin) * MoveMaxDistance / dis;   //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离
            transform.localPosition = vec;
        }
        if (Vector3.Distance(transform.localPosition, Origin) > ActiveMoveDistance)  //距离大于激活移动的距离
        {
            MovePosiNorm = (transform.localPosition - Origin).normalized;
            MovePosiNorm = new Vector3(MovePosiNorm.x, 0, MovePosiNorm.y);
        }
        else
            MovePosiNorm = Vector3.zero;
    }
    void MiouseDown()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
        }
        else
            mTrans.localPosition = Origin;
    }
    Vector3 result;
    private Vector3 _checkPosition(Vector3 movePos, Vector3 _offsetPos)
    {
        result = movePos + _offsetPos;
        return result;
    }

    void OnDrag(UnityEngine.EventSystems.PointerEventData evenData, GameObject go, EventTriggerListener etl)
    {
        if (!_drag)
        {
            _drag = true;
        }
        _deltaPos = evenData.delta;

        mTrans.localPosition += new Vector3(_deltaPos.x, _deltaPos.y, 0);
    }

    void OnDragOut(UnityEngine.EventSystems.PointerEventData evenData, GameObject go, EventTriggerListener etl)
    {
        _drag = false;
        mTrans.localPosition = Origin;
        if (PlayerMoveControl.moveEnd != null) PlayerMoveControl.moveEnd();
    }

    void OnMoveStart(UnityEngine.EventSystems.PointerEventData evenData,GameObject obj,EventTriggerListener etl)
    {
        if (PlayerMoveControl.moveStart != null) PlayerMoveControl.moveStart();
    }
}