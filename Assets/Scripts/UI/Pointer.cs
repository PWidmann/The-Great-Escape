using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Event for mousepointer
/// </summary>
public class Pointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent HoverTrigger;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverTrigger.Invoke();
        MainMenu.ControllerMovementStopped = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MainMenu.ControllerMovementStopped = false;
    }

    


    
}
