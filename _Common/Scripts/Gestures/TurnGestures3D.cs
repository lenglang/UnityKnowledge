using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
namespace WZK.Common
{
    /// <summary>
    /// 作者-wzk
    /// 功能-转动物体
    /// 使用说明-直接以组件形式添加到物体上，通过设置_isTurn的bool值来开启和禁用3D物体转动功能，设置_camera来指定照射相机，设置_turnDirection值来改变可转动的手势方向，设置_turnShaft值来改变物体沿着啥轴转动，设置_isGo值放开之后是否继续转动
    /// 注意事项-场景需添加EventSystem事件系统，照射相机需添加物理射线，3D物体需有Collider相关组件
    /// </summary>
    [AddComponentMenu("Common/Gestures/TurnGestures3D")]
    public class TurnGestures3D : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector]
        public int _turnDirection = 1;//鼠标（手指）转动方向0-无方向，1-顺时针，-1-逆时针
        [HideInInspector]
        public int _turnShaft = 2;//物体旋转轴0-x轴，1-y轴，2-z轴
        [HideInInspector]
        public Camera _camera;//照射相机
        [HideInInspector]
        public bool _isTurn = true;//是否可以转动
        [HideInInspector]
        public bool _isGo = false;//放开之后是否继续转动
        private bool _draging = false;//是否拖拽中
        private bool _isDown = false;//是否按下
        public Action<GameObject> _onBeginDrag;
        public Action<GameObject> _onEndDrag;
        public Action<GameObject> _onDown;
        public Action<GameObject> _onUp;
        public Action<float> _onTurnAngle;//角度委托变量
        private float _disAngle;//转动角度距离
        private Vector2 _originPosition;//物体坐标点
        private Vector2 _currentDragPosition;//当前鼠标（手指）拖拽的点
        private Vector2 _previousDragPosition;//上个鼠标（手指）拖拽的点
        private bool _isDragEnd = false;//拖拽结束
        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="evenData"></param>
        public void OnPointerDown(PointerEventData evenData)
        {
            if (_isDown || _isTurn == false) return;
            if (_camera == null)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("场景中缺少照射的主摄像机，将照射相机Tag设置为MainCamera或给该类_camera属性赋值照射摄像机");
                    return;
                }
                _originPosition = Camera.main.WorldToScreenPoint(this.transform.position);
            }
            else
            {
                _originPosition = _camera.WorldToScreenPoint(this.transform.position);
            }
            _isDown = true;
            _isDragEnd = false;
            _currentDragPosition = _previousDragPosition = evenData.position;
            if (_onDown != null) _onDown(gameObject);
        }
        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="evenData"></param>
        /// <param name="obj"></param>
        public void OnBeginDrag(PointerEventData evenData)
        {
            if (_isDown == false || _isTurn == false) return;
            _draging = true;
            if (_onBeginDrag != null) _onBeginDrag(gameObject);
        }
        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="evenData"></param>
        /// <param name="obj"></param>
        public void OnDrag(UnityEngine.EventSystems.PointerEventData evenData)
        {
            if (_isDown == false || _isTurn == false) return;
            _currentDragPosition = evenData.position;
            _disAngle = GetAngle2(_currentDragPosition, _originPosition) - GetAngle2(_previousDragPosition, _originPosition);
            if (IsRightDirection()) AngleUpdate(_disAngle);
            _previousDragPosition = _currentDragPosition;
        }
        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="evenData"></param>
        /// <param name="obj"></param>
        public void OnEndDrag(PointerEventData evenData)
        {
            if (_draging == false || _isTurn == false) return;
            _draging = false;
            if (_onEndDrag != null) _onEndDrag(gameObject);
            if (_isGo == false) return;
            if (IsRightDirection() && Math.Abs(_disAngle) > 3) _isDragEnd = true;
        }
        /// <summary>
        /// 是否正确转动
        /// </summary>
        /// <returns></returns>
        public bool IsRightDirection()
        {
            if (((_disAngle < 0 && _turnDirection == 1) || (_disAngle > 0 && _turnDirection == -1) || _turnDirection == 0) && Math.Abs(_disAngle) < 100)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 弹起
        /// </summary>
        /// <param name="evenData"></param>
        public void OnPointerUp(PointerEventData evenData)
        {
            if (_isDown == false || _isTurn == false) return;
            _isDown = false;
            if (_draging == false)
            {
                if (_onEndDrag != null) _onEndDrag(gameObject);
            }
        }
        /// <summary>
        /// 帧
        /// </summary>
        void Update()
        {
            if (_isDragEnd)
            {
                if (Math.Abs(_disAngle) > 0.5f)
                {
                    _disAngle = Mathf.Lerp(_disAngle, 0, 0.1f);
                    AngleUpdate(_disAngle);
                }
                else
                {
                    _isDragEnd = false;
                }
            }
        }
        /// <summary>
        ///  角度更新
        /// </summary>
        /// <param name="disAngle"></param>
        void AngleUpdate(float disAngle)
        {
            switch (_turnShaft)
            {
                case 0:
                    this.transform.Rotate(new Vector3(disAngle, 0, 0));
                    break;
                case 1:
                    this.transform.Rotate(new Vector3(0, disAngle, 0));
                    break;
                case 2:
                    this.transform.Rotate(new Vector3(0, 0, disAngle));
                    break;
                default:
                    break;
            }
            if (_onTurnAngle != null) _onTurnAngle(-disAngle);
        }
        /// <summary>
        /// 获取二维两点坐标角度
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private float GetAngle2(Vector2 p1, Vector2 p2)
        {
            float vx = p2.x - p1.x;
            float vy = p2.y - p1.y;
            float hyp = Mathf.Sqrt(Mathf.Pow(vx, 2) + Mathf.Pow(vy, 2));
            float rad = Mathf.Acos(vx / hyp);
            float angle = 180 / (Mathf.PI / rad);
            if (vy < 0)
            {
                angle = (-angle);
            }
            else if ((vy == 0) && vx < 0)
            {
                angle = 180;
            }
            return angle;
        }
    }
}
