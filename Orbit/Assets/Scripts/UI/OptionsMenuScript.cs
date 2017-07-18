using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField]
    private Toggle _bloomToggle;

    [SerializeField]
    private Slider _musicSlider;

    [SerializeField]
    private Slider _soundSlider;

    // Use this for initialization
    private void Start()
    {
        _bloomToggle.onValueChanged.AddListener( SetBloomEnabled );
        _soundSlider.onValueChanged.AddListener( SetSoundVolume );
        _musicSlider.onValueChanged.AddListener( SetMusicVolume );
    }

    private void OnEnable()
    {
        InitValues();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    private void InitValues()
    {
        if ( _bloomToggle )
            _bloomToggle.isOn = PlayerPrefs.GetInt( "BLOOM_EFFECT", 1 ) == 1;

        if ( _soundSlider )
            _soundSlider.value = PlayerPrefs.GetFloat( "SOUND_VOLUME", 1.0f );

        if ( _musicSlider )
            _musicSlider.value = PlayerPrefs.GetFloat( "MUSIC_VOLUME", 1.0f );
    }

    private void SetBloomEnabled( bool value )
    {
        PlayerPrefs.SetInt( "BLOOM_EFFECT", value ? 1 : 0 );
    }

    private void SetSoundVolume( float value )
    {
        PlayerPrefs.SetFloat( "SOUND_VOLUME", value );
    }

    private void SetMusicVolume( float value )
    {
        PlayerPrefs.SetFloat( "MUSIC_VOLUME", value );
    }
}