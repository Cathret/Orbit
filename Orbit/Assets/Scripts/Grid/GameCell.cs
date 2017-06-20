using System;
using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.Events;

public class GameCell : MonoBehaviour
{
    private static GameCell _lastSelected;

    public static GameCell SelectedCell
    {
        get {  return _lastSelected; }
    }

    public uint X { get; private set; }
    public uint Y { get; private set; }

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
                    SelectCallback();
                    break;
                case false:
                    UnselectCallback();
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

    public UnityEvent<Vector3> OnActionLaunched;

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
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * GameGrid.Instance.RotationSpeed);
    }

    public void SetPosition( uint x, uint y )
    {
        X = x;
        Y = y;

        GameGrid gameGrid = GameGrid.Instance;
        _targetPosition.x = (X + 0.5f) * gameGrid.CellSize;
        _targetPosition.y = (Y + 0.5f) * gameGrid.CellSize;
        _targetPosition.z = gameGrid.FixedZ;
    }

    void SetConnected(bool value)
    {
        if (_connected == value)
            return;
        _connected = value;
    }

    void SelectCallback()
    {
        if ( _selected )
            return;

        _selected = true;
        if ( _lastSelected )
            _lastSelected.Selected = false;

        _lastSelected = this;
        if ( OnSelection != null)
            OnSelection.Invoke(true);
    }

    void UnselectCallback()
    {
        if ( !_selected )
            return;

        _selected = false;
        _lastSelected = null;

        if (OnSelection != null)
            OnSelection.Invoke(false);
    }

    public static void Unselect()
    {
        _lastSelected.Selected = false;
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

    public void LaunchAction( Vector3 target )
    {
        if (OnActionLaunched != null)
            OnActionLaunched.Invoke(target);
    }
}
