using System.Collections.Generic;
using UnityEngine;
using Orbit.Entity;

public class OpponentManager : MonoBehaviour
{
    #region Members
    public CameraController CameraController
    {
        get { return _cameraController; }
        protected set { _cameraController = value; }
    }
    private CameraController _cameraController;

    [SerializeField]
    private uint _spawnPaddingFromScreen = 5;

    private List<AOpponentController> _listActiveOpponents = new List<AOpponentController>();
    #endregion

    private void Start()
    {
        CameraController = FindObjectOfType<CameraController>();
        if ( CameraController == null )
            Debug.LogError( "OpponentManager.Start() - could not find object of type CameraController" );
    }

    public void SpawnOpponent( AOpponentController opponentPrefab )
    {
        Vector3 distance = Random.insideUnitCircle.normalized;
        uint padding = GameGrid.Instance.EfficientSide;

        if ( _cameraController )
            padding = _cameraController.Padding;

        distance *= GameGrid.Instance.CellSize * ( padding + _spawnPaddingFromScreen );

        Vector3 position = GameGrid.Instance.RealCenter + distance;

        AOpponentController opponentInstance = Instantiate( opponentPrefab, position, Quaternion.identity );
        // TODO: change every event with sender as first parameter
        opponentInstance.TriggerDestroy += () => { _listActiveOpponents.Remove( opponentInstance ); };

        _listActiveOpponents.Add( opponentInstance );
    }

    public bool AreAllOpponentsDead()
    {
        return _listActiveOpponents.Count <= 0;
    }
}
