using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _downSprite, _upSprite;
    public UnityEvent OnClick;
    //public InputActionReference _confirm;

    //private void OnEnable()
    //{
    //    _confirm.action.started += OnConfirm;
    //    _confirm.action.performed += OnConfirmEnd;
    //}

    //private void OnConfirm(InputAction.CallbackContext obj)
    //{
    //    _image.sprite = _downSprite;
    //}

    //private void OnConfirmEnd(InputAction.CallbackContext obj)
    //{
    //    _image.sprite = _upSprite;
    //    OnClick?.Invoke();
    //}

    //private void OnDisable()
    //{
    //    _confirm.action.started -= OnConfirm;
    //    _confirm.action.performed -= OnConfirmEnd;
    //}

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
