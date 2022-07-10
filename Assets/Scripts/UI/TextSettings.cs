using System;
using UnityEngine;
using TMPro;


public class TextSettings : MonoBehaviour
{
    public static TextSettings Instance;
    public TMP_FontAsset SelectedFont;
    public TMP_FontAsset ClassicFont, DyslexicFriendlyFont;
    public bool IsClassicFontOn;

    public static event Action<TMP_FontAsset> OnFontChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeFont()
    {
        if (SelectedFont == ClassicFont)
        {
            SelectedFont = DyslexicFriendlyFont;
            IsClassicFontOn = false;
        }
        else
        {
            SelectedFont = ClassicFont;
            IsClassicFontOn = true;
        }
        OnFontChanged?.Invoke(SelectedFont);
    }

    public void Start()
    {
        OnFontChanged?.Invoke(SelectedFont);
    }
}