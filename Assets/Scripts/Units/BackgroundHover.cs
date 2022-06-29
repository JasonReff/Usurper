using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundHover : MonoBehaviour, IPointerEnterHandler
{
    public static Action OnBackgroundHover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnBackgroundHover?.Invoke();
    }
}