using UnityEngine;
using UnityEngine.EventSystems;
namespace WZK
{
    public class ScrollRectEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("点击了" + name);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("按下了" + name);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
    }
}
