using System.Collections.Generic;
using UnityEngine;
namespace WZK
{
    [DisallowMultipleComponent]
    public class ShowHideControl : MonoBehaviour
    {
        private static ShowHideControl _instance = null;
        public static ShowHideControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (new GameObject("显示隐藏管理")).AddComponent<ShowHideControl>();
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        public List<GameObjectGroup> _list = new List<GameObjectGroup>();
        [System.Serializable]
        public class GameObjectGroup
        {
            public string _desc = "描述";
            public List<GameObjectInformation> _list = new List<GameObjectInformation>();
            [System.Serializable]
            public class GameObjectInformation
            {
                public GameObject _obj;
                public bool _show = true;
            }
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="desc">描述</param>
        /// <param name="b">正执行还是反执行</param>
        public void Do(string desc, bool b = true)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i]._desc == desc)
                {
                    for (int j = 0; j < _list[i]._list.Count; j++)
                    {
                        if (b)
                        {
                            _list[i]._list[j]._obj.SetActive(_list[i]._list[j]._show);
                        }
                        else
                        {
                            _list[i]._list[j]._obj.SetActive(!_list[i]._list[j]._show);
                        }
                    }
                    break;
                }
            }
        }
    }
}
