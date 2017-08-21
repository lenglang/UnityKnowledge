using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace WZK
{
    public class UIDepth : MonoBehaviour
    {
        [Header("层级")]
        public int order;
        [Header("是否UI")]
        public bool isUI = false;
        [Header("UI是否可点击")]
        public bool isClick = false;
        void Start()
        {
            if (isUI)
            {
                Canvas canvas = GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = gameObject.AddComponent<Canvas>();
                    if (isClick) gameObject.AddComponent<GraphicRaycaster>();
                }
                canvas.overrideSorting = true;
                canvas.sortingOrder = order;
            }
            else
            {
                Renderer[] renders = GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renders)
                {
                    render.sortingOrder = order;
                }
            }
        }
    }
}