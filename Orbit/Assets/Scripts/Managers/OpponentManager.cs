using System.Collections.Generic;
using System.Linq;
using Orbit.Entity;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    private static OpponentManager _instance;
    public static OpponentManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<OpponentManager>();
            return _instance;
        }
    }

    #region Unity functions
    private void Start()
    {
        CameraController = FindObjectOfType<CameraController>();
        if ( CameraController == null )
            Debug.LogError( "OpponentManager.Start() - could not find object of type CameraController" );
    }
    #endregion

    #region Members
    public CameraController CameraController { get; protected set; }

    [SerializeField]
    private uint _spawnPaddingFromScreen = 5;

    private readonly List<AOpponentController> _listOpponentsAlive = new List<AOpponentController>();
    private readonly List<AOpponentController> _listOpponentsVisible = new List<AOpponentController>();
    #endregion

    #region Public functions
    public void SpawnOpponent( AOpponentController opponentPrefab )
    {
        Vector3 distance = Random.insideUnitCircle.normalized;
        uint padding = GameGrid.Instance.EfficientSide;

        if ( CameraController )
            padding = CameraController.Padding;

        distance *= GameGrid.Instance.CellSize * ( padding + _spawnPaddingFromScreen );

        Vector3 position = GameGrid.Instance.RealCenter + distance;

        AOpponentController opponentInstance = Instantiate( opponentPrefab, position, Quaternion.identity );
        // TODO: change every event with sender as first parameter
        opponentInstance.TriggerDestroy += () =>
        {
            _listOpponentsAlive.Remove( opponentInstance );
            if ( _listOpponentsVisible.Contains( opponentInstance ) )
                _listOpponentsVisible.Remove( opponentInstance );
        };

        _listOpponentsAlive.Add( opponentInstance );
    }

    public void SpawnOpponent( AOpponentController opponentPrefab, float radius )
    {
        Vector3 distance = new Vector3( Mathf.Cos( radius ), Mathf.Sin( radius ) );
        uint padding = GameGrid.Instance.EfficientSide;

        if ( CameraController )
            padding = CameraController.Padding;

        distance *= GameGrid.Instance.CellSize * ( padding + _spawnPaddingFromScreen );

        Vector3 position = GameGrid.Instance.RealCenter + distance;

        AOpponentController opponentInstance = Instantiate( opponentPrefab, position, Quaternion.identity );
        // TODO: change every event with sender as first parameter
        opponentInstance.TriggerDestroy += () =>
        {
            _listOpponentsAlive.Remove( opponentInstance );
            if ( _listOpponentsVisible.Contains( opponentInstance ) )
                _listOpponentsVisible.Remove( opponentInstance );
        };

        _listOpponentsAlive.Add( opponentInstance );
    }

    public void RegisterOpponent( AOpponentController opponent )
    {
        if ( !_listOpponentsVisible.Contains( opponent ) )
            _listOpponentsVisible.Add( opponent );
    }

    public bool AreAllOpponentsDead()
    {
        return _listOpponentsAlive.Count <= 0;
    }

    public bool FindClosestOpponent( Transform cell, out Vector3 target )
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = cell.position;

        foreach ( AOpponentController opponentController in _listOpponentsVisible )
        {
            float dist = Vector3.Distance( opponentController.transform.position, currentPos );
            if ( dist < minDist )
            {
                tMin = opponentController.transform;
                minDist = dist;
            }
        }

        if ( tMin )
        {
            target = tMin.position;
            return true;
        }

        target = new Vector3();
        return false;
    }

    public bool FindClosestOpponentInList( IEnumerable<AOpponentController> listOpponents, Transform cell
                                           , out AOpponentController target )
    {
        Transform tMin = null;
        AOpponentController bestOpponent = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = cell.position;

        foreach ( AOpponentController opponentController in listOpponents )
        {
            float dist = Vector3.Distance( opponentController.transform.position, currentPos );
            if ( dist < minDist )
            {
                bestOpponent = opponentController;
                tMin = opponentController.transform;
                minDist = dist;
            }
        }

        target = bestOpponent;

        return tMin;
    }

    public List<AOpponentController> GetOpponentsInQuarter( GameCell.Quarter quarter )
    {
        // TODO: need to be optimized (maybe have four lists?)
        return _listOpponentsVisible.Where( opponentController => opponentController.QuarterPosition == quarter )
                                    .ToList();
    }
    #endregion
}