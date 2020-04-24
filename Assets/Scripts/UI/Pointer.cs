using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent HoverTrigger = new UnityEvent();

    void Start()
    {
        HoverTrigger.AddListener(SoundManager.instance.PlayMenuPointerSoundFx);
    }

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
