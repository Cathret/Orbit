using System;
using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<WaveManager>();
            return _instance;
        }
    }
    private static WaveManager _instance = null;

    #region Members
    #region SerializeFields
    public uint NbWavesPerRound
    {
        get { return _nbWavesPerRound; }
        protected set { _nbWavesPerRound = value; }
    }
    [SerializeField]
    private uint _nbWavesPerRound = 3;

    public uint MaxNbQuarterPerWave
    {
        get { return _maxNbQuarterPerWave; }
        protected set { _maxNbQuarterPerWave = value; }
    }
    [SerializeField]
    private uint _maxNbQuarterPerWave = 1;

    protected uint SpawningSeed
    {
        get { return (uint)Random.Range( (int)_randomSeedSmall, (int)_randomSeedHigh ); }
    }
    [SerializeField]
    private uint _randomSeedSmall = 4;
    [SerializeField]
    private uint _randomSeedHigh = 7;

    public float MultiplicatorPerRound
    {
        get { return _multiplicatorPerRound; }
        protected set { _multiplicatorPerRound = value; }
    }
    [SerializeField, Range( 1.0f, 5.0f )]
    private float _multiplicatorPerRound = 1.3f;

    public float RoundLength
    {
        get { return _roundLength; }
        protected set { _roundLength = value; }
    }
    [SerializeField, Range( 0.0f, 300.0f )]
    private float _roundLength = 30.0f;

    [SerializeField]
    private AOpponentController[] _enemies;
    #endregion

    public float TimeBetweenWaves
    {
        get { return RoundLength / NbWavesPerRound; }
    }

    public uint CurrentRound
    {
        get { return _currentRound; }
        protected set { _currentRound = value; }
    }
    private uint _currentRound = 0;

    public uint CurrentWave
    {
        get { return _currentWave; }
        protected set { _currentWave = value; }
    }
    private uint _currentWave = 0;

    public float TimeSpentSinceLastWave
    {
        get { return _timeSpentSinceLastWave; }
        protected set
        {
            _timeSpentSinceLastWave = value;
            if ( _timeSpentSinceLastWave > TimeBetweenWaves )
            {
                OnUpdate = UpdateSendWave;
                _timeSpentSinceLastWave = 0.0f;
            }
        }
    }
    private float _timeSpentSinceLastWave = 0;

    public float TimeToNextWave
    {
        get { return TimeBetweenWaves -_timeSpentSinceLastWave; }
    }

    public delegate void DelegateRound( uint value );

    public event DelegateRound RoundChanged;

    public event Action<GameCell.Quarter> OnNewWave;

    public delegate void DelegateUpdate();

    public event DelegateUpdate OnUpdate = () => { };

    private OpponentManager _opponentManager = null;
    #endregion

    private void Awake()
    {
        if ( _randomSeedSmall > _randomSeedHigh )
            Debug.LogError( "WaveManager.Start() - RandomSeedSmall cannot be greater then RandomSeedHigh" );
    }

    private void Start()
    {
        _opponentManager = FindObjectOfType<OpponentManager>();
        if ( _opponentManager == null )
            Debug.LogError( "WaveManager.Start() - could not find object of type OpponentManager" );

        GameManager.Instance.OnAttackMode.AddListener( OnStartNewRound );
    }

    private void OnStartNewRound()
    {
        ++CurrentRound;

        if ( RoundChanged != null )
            RoundChanged.Invoke( CurrentRound );

        OnUpdate = UpdateSendWave;
    }

    private void Update()
    {
		if ( GameManager.Instance.CurrentGameState == GameManager.GameState.Play)
        	OnUpdate();
    }

    private void UpdateWaitForNextWave()
    {
        TimeSpentSinceLastWave +=
            Time.deltaTime; // Changes OnUpdate to UpdateSendWave when TimeSpentSinceLastWave go over the time between the waves
    }

    private void UpdateSendWave()
    {
        NextWave();

        if ( CurrentWave < NbWavesPerRound )
            OnUpdate = UpdateWaitForNextWave;
        else
            OnUpdate = UpdateWaitEndRound;
    }

    private void UpdateWaitEndRound()
    {
        if ( _opponentManager.AreAllOpponentsDead() )
        {
            CurrentWave = 0;
            TimeSpentSinceLastWave = 0;
            OnUpdate = () => { };
            GameManager.Instance.CurrentGameMode = GameManager.GameMode.Building;
        }
    }

    private void OnDestroy()
    {
    }

    private void NextWave()
    {
        uint nbOpponents = (uint)Mathf.FloorToInt( SpawningSeed * Mathf.Pow( MultiplicatorPerRound, CurrentRound ) );
        CurrentWave++;

        Array values = Enum.GetValues(typeof(GameCell.Quarter));
        GameCell.Quarter randomQuarter = (GameCell.Quarter)values.GetValue(Random.Range( 0, values.Length));

        SpawnEnemies( nbOpponents, randomQuarter );

        if ( OnNewWave != null )
            OnNewWave( randomQuarter );
    }

    private void SpawnEnemy( float radius )
    {
        int enemiesLength = _enemies.Length;
        if ( enemiesLength == 0 )
            return;

        AOpponentController prefab = _enemies[Random.Range( 0, enemiesLength )];

        _opponentManager.SpawnOpponent( prefab, radius );
    }

    private void SpawnEnemies( uint count, GameCell.Quarter quarter)
    {
        float rotation = 0.0f;
        float halfPI = Mathf.PI / 2;

        switch (quarter)
        {
            case GameCell.Quarter.TopRight:
                rotation = 0.0f;
                break;
            case GameCell.Quarter.BottomRight:
                rotation = halfPI * 3;
                break;
            case GameCell.Quarter.BottomLeft:
                rotation = halfPI * 2;
                break;
            case GameCell.Quarter.TopLeft:
                rotation = halfPI;
                break;
            default:
                throw new ArgumentOutOfRangeException("quarter", quarter, null);
        }

        for ( uint i = count; i != 0; --i )
        {
            float radius = Random.Range( 0, halfPI ) + rotation;
            SpawnEnemy(radius);
        }
    }
}