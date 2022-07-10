using TMPro;
using UnityEngine;

public class CustomText : MonoBehaviour
{
    private TextMeshProUGUI _textbox;
    private void OnEnable()
    {
        TextSettings.OnFontChanged += ChangeFont;
        if (TextSettings.Instance != null)
            ChangeFont(TextSettings.Instance.SelectedFont);
    }

    private void OnDisable()
    {
        TextSettings.OnFontChanged -= ChangeFont;
    }

    private void Start()
    {
        ChangeFont(TextSettings.Instance.SelectedFont);
    }

    private void Awake()
    {
        _textbox = GetComponent<TextMeshProUGUI>();
    }

    private void ChangeFont(TMP_FontAsset font)
    {
        _textbox.font = font;
    }
}