using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private float xSpeed, ySpeed;

    private void Update()
    {
        background.uvRect = new Rect(background.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, background.uvRect.size);
    }

}