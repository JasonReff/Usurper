using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _unitMovedClip, _unitDiedClip, _music;
    [SerializeField] private AudioSource _effectSource, _musicSource;
    [SerializeField] private float _minPitch, _maxPitch;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Unit.OnUnitMoved += PlayMoveSound;
        Unit.OnUnitDeath += PlayDeathSound;
    }

    private void OnDisable()
    {
        Unit.OnUnitMoved -= PlayMoveSound;
        Unit.OnUnitDeath -= PlayDeathSound;
    }
    private void PlayMoveSound()
    {
        PlaySoundEffect(_unitMovedClip);
    }

    private void PlayDeathSound(Unit unit)
    {
        PlaySoundEffect(_unitDiedClip);
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        _effectSource.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
        _effectSource.PlayOneShot(clip);
    }
}