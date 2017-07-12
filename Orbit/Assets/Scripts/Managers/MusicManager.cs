using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager _instance;

    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MusicManager>();
            return _instance;
        }
    }

    private enum MusicType
    {
        Attack,
        Build,
        Pause,
        GameOver
    }

    [SerializeField]
    private AudioClip _attackClip;

    [SerializeField]
    private AudioClip _buildClip;

    [SerializeField]
    private AudioClip _pauseClip;

    [SerializeField]
    private AudioClip _gameOverClip;

    private AudioSource _source;

    public AudioSource Source
    {
        get
        {
            if ( _source == null )
                _source = GetComponent<AudioSource>();
            if ( _source == null )
                _source = gameObject.AddComponent<AudioSource>();
            return _source;
        }
    }

    [SerializeField, Range(0.0f, 1.0f)]
    private float _volume = 1.0f;
    public float Volume
    {
        get { return _volume; }
        set
        {
            if ( value > 1.0f || value < 0.0f )
                return;
            _volume = value;
            Source.volume = value;
        }
    }

    private AudioClip _lastClip;

    // Use this for initialization
    void Start ()
    {
		GameManager.Instance.OnAttackMode.AddListener( PlayAttackMusic );
        GameManager.Instance.OnBuildMode.AddListener(PlayBuildMusic);
        GameManager.Instance.OnGameOver.AddListener(PlayGameOverMusic);
        GameManager.Instance.OnPlay.AddListener(ResumeMusic);
        GameManager.Instance.OnPause.AddListener(PlayPauseMusic);
    }

    public void PlayAttackMusic()
    {
        Play(MusicType.Attack);
    }

    public void PlayBuildMusic()
    {
        Play(MusicType.Build);
    }

    public void PlayPauseMusic()
    {
        Play(MusicType.Pause);
    }

    public void PlayGameOverMusic()
    {
        Play(MusicType.GameOver);
    }

    void Play( MusicType type )
    {
        AudioClip clip = GetMusic(type);
        if ( !clip )
            return;

        Source.clip = clip;
        Source.Play();
        _lastClip = clip;
    }

    AudioClip GetMusic( MusicType type )
    {
        switch ( type )
        {
            case MusicType.Attack:
                return _attackClip;
            case MusicType.Build:
                return _buildClip;
            case MusicType.Pause:
                return _pauseClip;
            case MusicType.GameOver:
                return _gameOverClip;
            default:
                return null;
        }
    }

    public void ResumeMusic()
    {
        switch (GameManager.Instance.CurrentGameMode)
        {
            case GameManager.GameMode.Attacking:
                PlayAttackMusic();
                break;
            case GameManager.GameMode.Building:
                PlayBuildMusic();
                break;
            default:
                Stop();
                break;
        }
    }

    public void PlayLastMusic()
    {
        if ( _lastClip )
        {
            AudioClip clip = Source.clip;
            Source.clip = _lastClip;
            Source.Play();
            _lastClip = clip;
        }
    }

    public void Stop()
    {
        Source.Stop();
    }
}
