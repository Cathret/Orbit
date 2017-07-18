using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void CountDelegate( uint count );

    public enum GameMode
    {
        Attacking
        , Building
        , None
    }

    public enum GameState
    {
        Play
        , Pause
        , GameOver
        , None
    }

    private static GameManager _instance;

    private GameMode _currentGameMode = GameMode.None;

    private GameState _currentGameState = GameState.None;

    private uint _resourcesCount;

    public UnityEvent OnAttackMode = new UnityEvent();
    public UnityEvent OnBuildMode = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnNone = new UnityEvent();
    public UnityEvent OnNoneMode = new UnityEvent();
    public UnityEvent OnPause = new UnityEvent();

    public UnityEvent OnPlay = new UnityEvent();

    public static GameManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            if ( value == _currentGameState )
                return;
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

    public bool Playing
    {
        get { return _currentGameState == GameState.Play; }
    }
    public GameMode CurrentGameMode
    {
        get { return _currentGameMode; }
        set
        {
            if ( value == _currentGameMode )
                return;
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

    public event CountDelegate OnResourcesChange;

    // Use this for initialization
    private void Start()
    {
        CurrentTime = Time.time;
        CurrentGameMode = GameMode.Building;
        CurrentGameState = GameState.Play;

        GameGrid.Instance.OnGridEmpty.AddListener( GameOver );
    }

    // Update is called once per frame
    private void Update()
    {
        if ( CurrentGameState == GameState.Play )
            CurrentTime += Time.deltaTime;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene( 0 );
    }

    public void Restart()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }

    private void GameOver()
    {
        CurrentGameState = GameState.GameOver;
    }

    private void OnApplicationFocus( bool hasFocus )
    {
        if ( !hasFocus )
            CurrentGameState = GameState.Pause;
    }

    private void OnApplicationPause( bool pauseStatus )
    {
        CurrentGameState = pauseStatus ? GameState.Pause : GameState.Play;
    }
}