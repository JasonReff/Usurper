using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderRelease : MonoBehaviour, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private Slider _slider;

    public UnityEvent<float> OnHandleReleased;

    public void OnPointerUp(PointerEventData eventData)
    {
        OnHandleReleased?.Invoke(_slider.value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnHandleReleased?.Invoke(_slider.value);
    }
}
