using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSettings _settings;
    [SerializeField] private AudioClip _unitMovedClip, _unitDiedClip, _coinClip, _unitPlacedClip, _music;
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
        BoardTile.OnUnitPlaced += PlayPlacementSound;
        ShopUI.OnCoinGained += PlayCoinSound;
        AudioSettings.OnAudioSettingsChanged += AdjustVolume;
    }

    private void OnDisable()
    {
        Unit.OnUnitMoved -= PlayMoveSound;
        Unit.OnUnitDeath -= PlayDeathSound;
        ShopUI.OnCoinGained -= PlayCoinSound;
        BoardTile.OnUnitPlaced -= PlayPlacementSound;
        AudioSettings.OnAudioSettingsChanged -= AdjustVolume;
    }

    private void PlayMoveSound()
    {
        PlaySoundEffect(_unitMovedClip);
    }

    private void PlayDeathSound(Unit unit)
    {
        PlaySoundEffect(_unitDiedClip);
    }

    private void PlayCoinSound()
    {
        PlaySoundEffect(_coinClip);
    }

    private void PlayPlacementSound(Unit unit)
    {
        if (unit.UnitData.IsKing == false)
            PlaySoundEffect(_unitPlacedClip);
    }

    public void PlaySampleSound()
    {
        int random = UnityEngine.Random.Range(0, 3);
        if (random == 0)
            PlaySoundEffect(_unitDiedClip);
        if (random == 1)
            PlaySoundEffect(_unitMovedClip);
        if (random == 2)
            PlaySoundEffect(_unitPlacedClip);
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        _effectSource.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
        _effectSource.PlayOneShot(clip);
    }

    private void AdjustVolume()
    {
        _effectSource.volume = _settings._effectsVolume;
        _musicSource.volume = _settings._musicVolume;
    }
}