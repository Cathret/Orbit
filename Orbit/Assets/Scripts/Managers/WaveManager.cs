using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private static WaveManager _instance;

    public static WaveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<WaveManager>();
            return _instance;
        }
    }

    [SerializeField]
    private AOpponentController[] _enemies;

    [SerializeField]
    private uint _currentWave = 1;

    [SerializeField, Range(0, 3600)]
    private float _waveLength = 30.0f;

    private float _timeLeft = 0;

    public float TimeLeft
    {
        get { return _timeLeft; }
        set
        {
            _timeLeft = value;
            if ( _timeLeft > _waveLength )
            {
                NextWave();
                _timeLeft = 0.0f;
            }
        }
    }

    [SerializeField]
    private uint _paddingFromCenter = 8;

    [SerializeField]
    private uint _baseEnemyPerSec = 10;

    [SerializeField, Range( 1.0f, 5.0f)]
    private float _multiplyPerWave = 1.3f;

    private uint _enemyPerSec = 10;

    public delegate void WaveDelegate( uint value );
    public event WaveDelegate OnWaveChanged;

    protected Coroutine _spawnerEnemies;

    // Use this for initialization
    void Start ()
    {
        _spawnerEnemies = StartCoroutine(EnemyCoroutine());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ( GameManager.Instance.CurrentState == GameManager.State.PLAYING )
        {
            TimeLeft += Time.deltaTime;
        }
	}

    void OnDestroy()
    {
        if (_spawnerEnemies != null)
            StopCoroutine(_spawnerEnemies);
    }

    void NextWave()
    {
        _enemyPerSec = ( uint )( _baseEnemyPerSec * Mathf.Pow( _multiplyPerWave, _currentWave++ ) );

        if (OnWaveChanged != null)
            OnWaveChanged.Invoke(_currentWave);
    }

    void SpawnEnemy()
    {
        int enemiesLength = _enemies.Length;
        if (enemiesLength == 0)
            return;

        AOpponentController prefab = _enemies[Random.Range( 0, enemiesLength )];

        Vector3 distance = Random.insideUnitCircle.normalized;
        distance *= GameGrid.Instance.CellSize * ( GameGrid.Instance.EfficientSide + _paddingFromCenter );

        Vector3 position = GameGrid.Instance.RealCenter + distance;

        Instantiate( prefab, position, Quaternion.identity );
    }

    IEnumerator EnemyCoroutine()
    {
        while ( true )
        {
            if ( GameManager.Instance.CurrentState == GameManager.State.PLAYING )
                SpawnEnemy();
            yield return new WaitForSeconds(1.0f / _enemyPerSec);
        }
    }
}
