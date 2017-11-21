using UnityEngine;
using System.Collections.Generic;
namespace WZK
{
    public class Rope : MonoBehaviour
    {
        [Header("开始控制点集")]
        public List<Transform> _startPointList=new List<Transform>();
        [Header("结束控制点集")]
        public List<Transform> _endPointList=new List<Transform>();
        [Header("绳子点集")]
        public List<Transform> _ropePointList = new List<Transform>();
        [Header("线段之间的间隙")]
        public float _dis = 0.1f;//绘制的0-1的间隙 越小曲线越接近曲线，
        [Header("曲线控制点")]
        public Transform a0;
        [Header("曲线中心点")]
        public Transform _centerPoint;//中心点
        [Header("碰撞忽略")]
        public List<GameObject> _ignoreCollisionList = new List<GameObject>();
        private Transform v0, v1;//曲线两端
        private int _startLength;//开始点集长度
        private int _endLength;//结束点集长度
        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            _startLength = _startPointList.Count;
            _endLength = _endPointList.Count;
            v0 = _startPointList[_startLength - 1];
            v1 = _endPointList[_endLength - 1];
            //BoxCollider bc;
            //BoxCollider bc0 = _centerPoint.GetComponent<BoxCollider>();
            //BoxCollider bc1= a0.GetComponent<BoxCollider>();
            //for (int i = 0; i < _ignoreCollisionList.Count; i++)
            //{
            //    bc = _ignoreCollisionList[i].GetComponent<BoxCollider>();
            //    Physics.IgnoreCollision(bc0, bc);
            //    Physics.IgnoreCollision(bc1, bc);
            //}
            UpdateLine();
        }
        /// <summary>
        /// 更新开始和结束
        /// </summary>
        private void UpdateStartEnd()
        {
            for (int i = 0; i < _startLength; i++)
            {
                _ropePointList[i].position = _startPointList[i].position;
            }
            for (int i = 0; i < _endLength; i++)
            {

                _ropePointList[_ropePointList.Count-1-i].position = _endPointList[i].position;
            }
        }
        /// <summary>
        /// 更新线
        /// </summary>
        public void UpdateLine()
        {
            //更新中心点位置
            _centerPoint.localPosition = new Vector3((v0.position.x+v1.position.x)/2, (v0.position.y + v1.position.y)/2, (v0.position.z + v1.position.z)/2);
            CreateLine();
        }
        private void Update()
        {
            UpdateLine();
        }
        //生成线
        private void CreateLine()
        {
            int count = 0;
            for (float i = 0; i < 1; i += _dis)
            {
                //Debug.DrawLine(po(i, v0, v1, a0), po(i + _dis, v0, v1, a0), Color.red);
                //Debug.DrawLine(v0.transform.position, a0.transform.position, Color.green);
                //Debug.DrawLine(a0.transform.position, v1.transform.position, Color.green);
                if (count >= _startLength && count <= _ropePointList.Count - 1)
                {
                    _ropePointList[count].position = po(i, v0, v1, a0);
                }
                count++;
            }
            UpdateStartEnd();
        }
        private Vector3 po(float t, Transform v0, Transform v1, Transform a0)//根据当前时间t 返回路径  其中v0为起点 v1为终点 a为中间点   
        {
            Vector3 a;
            a.x = t * t * (v1.position.x - 2 * a0.position.x + v0.position.x) + v0.position.x + 2 * t * (a0.position.x - v0.position.x);//公式为B(t)=(1-t)^2*v0+2*t*(1-t)*a0+t*t*v1 其中v0为起点 v1为终点 a为中间点   
            a.y = t * t * (v1.position.y - 2 * a0.position.y + v0.position.y) + v0.position.y + 2 * t * (a0.position.y - v0.position.y);
            a.z = t * t * (v1.position.z - 2 * a0.position.z + v0.position.z) + v0.position.z + 2 * t * (a0.position.z - v0.position.z);
            return a;
        }
    }
}
