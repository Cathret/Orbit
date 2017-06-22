using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    public enum GameState
    {
        Play,
        Pause,
        GameOver,
        None
    }

    public enum GameMode
    {
        Attacking,
        Building,
        None
    }

    public UnityEvent OnPlay = new UnityEvent();
    public UnityEvent OnPause = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnNone = new UnityEvent();

    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            switch ( value )
            {
                case GameState.Play:
                    if ( OnPlay != null )
                        OnPlay.Invoke();
                    break;
                case GameState.Pause:
                    if ( OnPause != null )
                        OnPause.Invoke();
                    break;
                case GameState.GameOver:
                    if ( OnGameOver != null )
                        OnGameOver.Invoke();
                    break;
                case GameState.None:
                    if ( OnNone != null )
                        OnNone.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException( "value", value, null );
            }
            _currentGameState = value;
        }
    }

    public UnityEvent OnAttackMode = new UnityEvent();
    public UnityEvent OnBuildMode = new UnityEvent();
    public UnityEvent OnNoneMode = new UnityEvent();

    private GameMode _currentGameMode = GameMode.None;
    public GameMode CurrentGameMode
    {
        get { return _currentGameMode; }
        set
        {
            switch ( value )
            {
                case GameMode.Attacking:
                    if ( OnAttackMode != null )
                        OnAttackMode.Invoke();
                    break;
                case GameMode.Building:
                    if ( OnBuildMode != null )
                        OnBuildMode.Invoke();
                    break;
                case GameMode.None:
                    if ( OnNoneMode != null )
                        OnNoneMode.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException( "value", value, null );
            }
            _currentGameMode = value;
        }
    }

    public float CurrentTime { get; private set; }

    private uint _resourcesCount = 0;

    public delegate void CountDelegate( uint count );

    public event CountDelegate OnResourcesChange;

    public uint ResourcesCount
    {
        get { return _resourcesCount; }
        set
        {
            _resourcesCount = value;
            if ( OnResourcesChange != null )
                OnResourcesChange.Invoke( _resourcesCount );
        }
    }

    // Use this for initialization
    void Start()
    {
        CurrentTime = Time.time;
        CurrentGameMode = GameMode.Building;
        CurrentGameState = GameState.Play;
    }

    // Update is called once per frame
    void Update()
    {
        if ( CurrentGameState == GameState.Play )
            CurrentTime += Time.deltaTime;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene( SceneManager.GetSceneByName( "BaseScene" ).buildIndex );
    }

    public void Restart()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }
}