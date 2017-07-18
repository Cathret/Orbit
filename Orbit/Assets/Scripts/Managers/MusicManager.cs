using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;

    [SerializeField]
    private AudioClip _attackClip;

    [SerializeField]
    private AudioClip _buildClip;

    [SerializeField]
    private AudioClip _gameOverClip;

    private AudioClip _lastClip;

    [SerializeField]
    [Range( 0.0f, 1.0f )]
    private float _musicVolume = 1.0f;

    [SerializeField]
    private AudioClip _pauseClip;

    [SerializeField]
    [Range( 0.0f, 1.0f )]
    private float _soundVolume = 1.0f;

    private AudioSource _source;

    public static MusicManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<MusicManager>();
            return _instance;
        }
    }

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
    public float SoundVolume
    {
        get { return _soundVolume; }
    }
    public float MusicVolume
    {
        get { return _musicVolume; }
    }

    private void Awake()
    {
        _soundVolume = PlayerPrefs.GetFloat( "SOUND_VOLUME" );
        _musicVolume = PlayerPrefs.GetFloat( "MUSIC_VOLUME" );
        Source.volume = MusicVolume;
    }

    // Use this for initialization
    private void Start()
    {
        GameManager.Instance.OnAttackMode.AddListener( PlayAttackMusic );
        GameManager.Instance.OnBuildMode.AddListener( PlayBuildMusic );
        GameManager.Instance.OnGameOver.AddListener( PlayGameOverMusic );
        GameManager.Instance.OnPlay.AddListener( ResumeMusic );
        GameManager.Instance.OnPause.AddListener( PlayPauseMusic );
    }

    public void PlayAttackMusic()
    {
        Play( MusicType.Attack );
    }

    public void PlayBuildMusic()
    {
        Play( MusicType.Build );
    }

    public void PlayPauseMusic()
    {
        Play( MusicType.Pause );
    }

    public void PlayGameOverMusic()
    {
        Play( MusicType.GameOver );
    }

    private void Play( MusicType type )
    {
        AudioClip clip = GetMusic( type );
        if ( !clip )
            return;

        Source.clip = clip;
        Source.Play();
        _lastClip = clip;
    }

    private AudioClip GetMusic( MusicType type )
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
        switch ( GameManager.Instance.CurrentGameMode )
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

    private enum MusicType
    {
        Attack
        , Build
        , Pause
        , GameOver
    }
}