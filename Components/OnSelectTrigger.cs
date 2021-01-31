using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnSelectTrigger : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private UnityEvent _OnSelect;


    public void OnSelect(BaseEventData eventData)
    {
        _OnSelect?.Invoke();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
    
}
