using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _downSprite, _upSprite;
    public UnityEvent OnClick;
    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _downSprite;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _upSprite;
        OnClick?.Invoke();
    }
}
