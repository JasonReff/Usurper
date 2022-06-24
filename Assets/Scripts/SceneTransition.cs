using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void FadeOut()
    {
        Instance._animator.SetTrigger("FadeOut");
    }

    public static void FadeIn()
    {
        Instance._animator.SetTrigger("FadeIn");
    }
}