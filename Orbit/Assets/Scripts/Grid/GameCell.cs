using System;
using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.Events;

public class GameCell : MonoBehaviour
{
    private static GameCell _lastSelected;

    public uint X { get; private set; }
    public uint Y { get; private set; }

    public GameGrid Grid;

    private Vector3 _targetPosition;

    private bool _connected = false;
    private bool _selected = false;

    public bool Selected
    {
        get { return _selected; }
        set
        {
            switch ( value )
            {
                case true: 
                    Select();
                    break;
                case false:
                    Unselect();
                    break;
            }
            _selected = value;
        }    
    }

    public UnityEvent<bool> OnSelection;

    public bool Connected
    {
        get { return _connected; }
        set { SetConnected(value); }
    }

    private AUnitController _unit;
    public AUnitController Unit
    {
        get
        {
            if ( _unit == null )
                _unit = GetComponent<AUnitController>();
            return _unit;
        }
    }

    void Awake()
    {
        _targetPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Grid)
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * Grid.RotationSpeed);
    }

    public void SetPosition(GameGrid grid, uint x, uint y)
    {
        Grid = grid;
        X = x;
        Y = y;

        _targetPosition.x = (X + 0.5f) * Grid.CellSize;
        _targetPosition.y = (Y + 0.5f) * Grid.CellSize;
        _targetPosition.z = Grid.FixedZ;
    }

    void SetConnected(bool value)
    {
        if (_connected == value)
            return;
        _connected = value;
    }

    void Select()
    {
        if (_lastSelected)
            _lastSelected.Unselect();
        _selected = true;
        _lastSelected = this;
        if ( OnSelection != null)
            OnSelection.Invoke(true);
    }

    void Unselect()
    {
        _selected = false;
        _lastSelected = null;
        if (OnSelection != null)
            OnSelection.Invoke(false);
    }

    public bool IsConnectedTo( GameCell cell )
    {
        if ( cell == this )
            return false;
        if ( X == cell.X )
        {
            if ( Math.Abs( Y - cell.Y ) == 1 )
                return true;
        }
        else if ( Y == cell.Y)
        {
            if (Math.Abs(X - cell.X) == 1)
                return true;
        }
        return false;
    }
}
