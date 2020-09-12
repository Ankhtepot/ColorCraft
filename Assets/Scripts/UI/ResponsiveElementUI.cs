#pragma warning disable 0414

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class ResponsiveElementUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
    {
#pragma warning disable 649
        [SerializeField] private bool isOver;
        [SerializeField] private UnityEvent MouseOver;
        [SerializeField] private UnityEvent MouseLeave;
#pragma warning restore 649
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            isOver = true;
            MouseOver.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOver = false;
            MouseLeave.Invoke();
        }
    }
}
