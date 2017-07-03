using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
namespace WZK.Common
{
    /// <summary>
    /// 作者-wzk
    /// 功能-滑动手势
    /// 使用说明-直接以组件形式添加到物体上，通过设置_isSlide的bool值来开启和禁用滑动功能,注意感应的区域！！！！！！！！！
    /// </summary>
    [AddComponentMenu("Gestures/SlideGestures")]
    public class SlideGestures : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public enum Direction
        {
            从下到上,
            从上到下,
            从右到左,
            从左到右
        }
        [HideInInspector]
        public Direction direction = Direction.从右到左;//滑动方向
        [HideInInspector]
        public float _disX = Screen.height / 6;//X方向滑动多少距离算成功
        [HideInInspector]
        public float _disY = Screen.height / 8;//Y方向滑动多少距离算成功
        [HideInInspector]
        public bool _isSlide = true;//是否可以滑动
        public Action<GameObject> _onBeginDrag;//开始拖拽委托方法
        public Action<GameObject> _onComplete;//完成委托方法
        private float _dx = 0;//x差值
        private float _dy = 0;//y差值
        private float _dis = 0;//距离
        private Vector2 _startDragPosition;//开始拖拽点
        private Vector2 _currentDragPosition;//当前拖拽点
        private Vector2 _previousDragPosition;//上一个拖拽点
                                              /// <summary>
                                              /// 开始拖拽
                                              /// </summary>
                                              /// <param name="evenData"></param>
                                              /// <param name="obj"></param>
        public void OnBeginDrag(PointerEventData evenData)
        {
            if (_isSlide == false) return;
            _startDragPosition = _previousDragPosition = _currentDragPosition = evenData.position;
            if (_onBeginDrag != null) _onBeginDrag(gameObject);
        }
        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="evenData"></param>
        /// <param name="obj"></param>
        public void OnDrag(PointerEventData evenData)
        {
            if (_isSlide == false) return;
            _currentDragPosition = evenData.position;
            Direction dragDir = JudgeDirection(_currentDragPosition, _previousDragPosition);
            if (dragDir == direction)
            {
                if (dragDir == Direction.从下到上 || dragDir == Direction.从上到下)
                {
                    _dis = _currentDragPosition.y - _startDragPosition.y;
                    if (Math.Abs(_dis) > _disY && ((dragDir == Direction.从下到上 && _dis > 0) || (dragDir == Direction.从上到下 && _dis < 0)))
                    {
                        if (_onComplete != null) _onComplete(gameObject);
                    }
                }
                else
                {
                    _dis = _currentDragPosition.x - _startDragPosition.x;
                    if (Math.Abs(_dis) > _disX && ((dragDir == Direction.从左到右 && _dis > 0) || (dragDir == Direction.从右到左 && _dis < 0)))
                    {
                        if (_onComplete != null) _onComplete(gameObject);
                    }
                }
            }
            _previousDragPosition = _currentDragPosition;
        }
        /// <summary>
        /// 判断移动方向
        /// </summary>
        /// <param name="currentDir"></param>
        /// <param name="previousDir"></param>
        /// <returns></returns>
        public Direction JudgeDirection(Vector2 currentDir, Vector2 previousDir)
        {
            _dx = currentDir.x - previousDir.x;
            _dy = currentDir.y - previousDir.y;
            if (Math.Abs(_dy) > Math.Abs(_dx))
            {
                if (_dy > 0)
                {
                    return Direction.从下到上;
                }
                else
                {
                    return Direction.从上到下;
                }
            }
            else
            {
                if (_dx > 0)
                {
                    return Direction.从左到右;
                }
                else
                {
                    return Direction.从右到左;
                }
            }
        }
    }
}
