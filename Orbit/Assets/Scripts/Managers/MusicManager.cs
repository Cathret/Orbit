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

    [SerializeField]
    private OrbitMusic[] musics;

    public enum MusicType
    {
        Attack,
        Build,
        Pause,
        GameOver
    }

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
        AudioClip clip = GetMusic(MusicType.Attack);
        if ( !clip )
            return;

        Source.clip = clip;
        Source.Play();
        _lastClip = clip;
    }

    AudioClip GetMusic( MusicType type )
    {
        foreach ( OrbitMusic music in musics )
        {
            if ( type == music.Type )
                return music.Music;
        }
        return null;
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

    [Serializable]
    public class OrbitMusic
    {
        
        [SerializeField]
        private AudioClip _music;

        public AudioClip Music
        {
            get { return _music; }
        }

        [SerializeField]
        private MusicType _type;
        public MusicType Type
        {
            get { return _type; }
        }

    }
}
