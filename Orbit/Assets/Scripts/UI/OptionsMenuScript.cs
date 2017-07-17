using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField]
    private Toggle _bloomToggle;

    [SerializeField]
    private Slider _soundSlider;

    [SerializeField]
    private Slider _musicSlider;

    // Use this for initialization
    void Start ()
    {
        _bloomToggle.onValueChanged.AddListener(SetBloomEnabled);
        _soundSlider.onValueChanged.AddListener(SetSoundVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    void OnEnable()
    {
        InitValues();
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    } 

    void InitValues()
    {
        if ( _bloomToggle )
        {
            _bloomToggle.isOn = PlayerPrefs.GetInt( "BLOOM_EFFECT", 1 ) == 1;
        }

        if (_soundSlider)
        {
            _soundSlider.value = PlayerPrefs.GetFloat("SOUND_VOLUME", 1.0f);
        }

        if (_musicSlider)
        {
            _musicSlider.value = PlayerPrefs.GetFloat("MUSIC_VOLUME", 1.0f);
        }
    }

    void SetBloomEnabled( bool value )
    {
        PlayerPrefs.SetInt("BLOOM_EFFECT", value ? 1 : 0);
    }

    void SetSoundVolume( float value )
    {
        PlayerPrefs.SetFloat("SOUND_VOLUME", value);
    }

    void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MUSIC_VOLUME", value);
    }
}
