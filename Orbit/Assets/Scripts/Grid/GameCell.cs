using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCell : MonoBehaviour
{
    private uint X;
    private uint Y;

    public GameGrid _grid;

    private Vector3 _targetPosition;

    private bool connected = false;
    private bool selected = false;

    public bool Connected
    {
        get { return connected;}
        set { SetConnected(value); }
    }

    void Awake()
    {
        _targetPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_grid)
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _grid.RotationSpeed);
    }

    public void SetPosition(GameGrid grid, uint x, uint y)
    {
        _grid = grid;
        X = x;
        Y = y;

        _targetPosition.x = x * _grid.CellSize;
        _targetPosition.y = y * _grid.CellSize;
        _targetPosition.z = _grid.FixedX;
    }

    void SetConnected(bool value)
    {
        if (connected == value)
            return;
        connected = value;
    }

}
