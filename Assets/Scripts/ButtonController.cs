using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonOver);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
    }
}
