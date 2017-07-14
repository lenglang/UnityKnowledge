using UnityEngine;
using System;
using UnityEngine.EventSystems;
namespace WZK.Common
{
    /// <summary>
    /// 作者-wzk
    /// 功能-3D物体地面水平拖拽
    /// 使用说明-直接以组件形式添加到物体上，通过设置_isDrag的bool值来开启和禁用3D物体拖拽功能，设置_camera来指定照射相机
    /// 注意事项-场景需添加EventSystem事件系统，照射相机需添加物理射线，3D物体需有Collider相关组件
    /// </summary>
    [AddComponentMenu("Common/Gestures/DragGestures3D2")]
    public class DragGestures3D2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("检测层")]
        public string _layerMask = "";
        [HideInInspector]
        public Camera _camera;//照射相机
        [HideInInspector]
        public bool _isDrag = true;//是否可以拖拽
        [HideInInspector]
        public PointerEventData _pointerEventData;//事件数据
        [HideInInspector]
        public Vector3 _offset;//偏移点
        public Action<GameObject> _onDownBefore;//按下前委托动作
        public Action<GameObject> _onDown;//按下委托动作
        public Action<GameObject> _onBeginDrag;//开始拖拽委托动作
        public Action<GameObject> _onDrag;//拖拽中委托动作
        public Action<GameObject> _onEndDrag;//结束拖拽委托动作
        private Vector3 _screenSpace;//屏幕坐标
        private bool _draging = false;//是否拖拽中
        private bool _isDown = false;//是否按下
        public void OnPointerDown(PointerEventData evenData)
        {
            if(string.IsNullOrEmpty(_layerMask)) Debug.LogError("未设置检测层：_layerMask");
            if (_isDown || _isDrag == false) return;
            if (_onDownBefore != null) _onDownBefore(gameObject);
            Ray ray;
            if (_camera == null)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("场景中缺少照射的主摄像机，将照射相机Tag设置为MainCamera或给该类_camera属性赋值照射摄像机");
                }
                ray = Camera.main.ScreenPointToRay(evenData.position);
            }
            else
            {
                ray = _camera.ScreenPointToRay(evenData.position);
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask(_layerMask)))
            {
                _offset.x = this.transform.position.x- hit.point.x;
                _offset.z = this.transform.position.z- hit.point.z;
            }
            _isDown = true;
            _pointerEventData = evenData;
            if (_onDown != null) _onDown(gameObject);
        }
        public void OnBeginDrag(PointerEventData evenData)
        {
            if (_isDown == false || _isDrag == false) return;
            _draging = true;
            UpdatePosition(evenData);
            if (_onBeginDrag != null) _onBeginDrag(gameObject);
        }
        public void OnDrag(PointerEventData evenData)
        {
            if (_isDown == false || _isDrag == false) return;
            UpdatePosition(evenData);
            if (_onDrag != null) _onDrag(gameObject);
        }
        public void UpdatePosition(PointerEventData evenData)
        {
            _pointerEventData = evenData;
            Ray ray;
            if (_camera == null)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("场景中缺少照射的主摄像机，将照射相机Tag设置为MainCamera或给该类_camera属性赋值照射摄像机");
                }
                ray = Camera.main.ScreenPointToRay(evenData.position);
            }
            else
            {
                ray = _camera.ScreenPointToRay(evenData.position);
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f,LayerMask.GetMask(_layerMask)))
            {
                this.transform.position = new Vector3(hit.point.x + _offset.x,this.transform.position.y, hit.point.z + _offset.z);
            }
        }
        public void OnEndDrag(PointerEventData evenData)
        {
            //有拖动才会执行
            if (_draging == false || _isDrag == false) return;
            _draging = false;
            _pointerEventData = evenData;
            if (_onEndDrag != null) _onEndDrag(gameObject);
        }
        public void OnPointerUp(PointerEventData evenData)
        {
            if (_isDown == false || _isDrag == false) return;
            _isDown = false;
            _pointerEventData = evenData;
            if (_draging == false)
            {
                if (_onEndDrag != null) _onEndDrag(gameObject);
            }
        }
        void OnApplicationPause(bool isPause)
        {
            if (isPause)
            {
                //游戏暂停-缩到桌面的时候触发
            }
            else
            {
                //游戏开始-回到游戏的时候触发
                if (_onEndDrag != null && _isDown && _isDrag) _onEndDrag(gameObject);
            }
        }
    }
}
