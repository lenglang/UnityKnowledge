using UnityEngine;
using System;
using UnityEngine.EventSystems;
namespace WZK
{
    /// <summary>
    /// 作者-wzk
    /// 功能-UI拖拽
    /// 使用说明-直接以组件形式添加到物体上，通过设置_isDrag的bool值来开启和禁用UI拖拽功能
    /// </summary>
    [AddComponentMenu("Common/Gestures/DragGestures2D")]
    public class DragGestures2D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector]
        public bool _isDrag = true;//设置是否可以被拖拽
        public Action<GameObject> _onDown;//按下委托动作
        public Action<GameObject> _onBeginDrag;//开始拖拽委托动作
        public Action<GameObject> _onDrag;//拖拽中委托动作
        public Action<GameObject> _onEndDrag;//结束拖拽委托动作
        private Vector3 _localPosition;//按下UI时UI的位置
        private bool _isDown = false;//是否按下
        private bool _draging = false;//是否拖拽中
        private Vector2 _startPosition;//鼠标（手指）按下点
        private Vector2 _currentPosition;//鼠标（手指）当前点
                                         // Use this for initialization
        public void OnPointerDown(PointerEventData evenData)
        {
            if (_isDown || _isDrag == false) return;
            _isDown = true;
            _localPosition = this.transform.localPosition;
            _startPosition = evenData.position;
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
            _currentPosition = evenData.position;
            this.transform.localPosition = new Vector2(_localPosition.x + _currentPosition.x - _startPosition.x, _localPosition.y + _currentPosition.y - _startPosition.y);
        }
        public void OnEndDrag(PointerEventData evenData)
        {
            //有拖动才会执行
            if (_draging == false || _isDrag == false) return;
            _draging = false;
            if (_onEndDrag != null) _onEndDrag(gameObject);
        }
        public void OnPointerUp(PointerEventData evenData)
        {
            if (_isDown == false || _isDrag == false) return;
            _isDown = false;
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

