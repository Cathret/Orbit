using System;
using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.Events;

public class GameCell : MonoBehaviour
{

    public uint X { get; private set; }
    public uint Y { get; private set; }

    private Vector3 _targetPosition;

    public delegate void DelegateBool( bool value );

    public event DelegateBool OnSelection;

    public delegate void DelegateVector3( Vector3 value );

    public DelegateVector3 OnActionLaunched;
    public DelegateVector3 OnDraggedActionLaunched;

    private bool _connected = false;
    private bool _selected = false;

    private string _frameName = "Frame_Cell";

    private SpriteRenderer _spriteRenderer;

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

    public bool Connected
    {
        get { return _connected; }
        set { SetConnected( value ); }
    }

    private AUnitController _unit;
    public AUnitController Unit
    {
        get
        {
            if (_unit == null)
                _unit = GetComponent<AUnitController>();
            return _unit;
        }
    }

    public enum Quarter
    {
        TopRight, BottomRight, BottomLeft, TopLeft
    }

    public Quarter QuarterPosition
    {
        get
        {
            GameGrid gameGrid = GameGrid.Instance;
            if (Y >= gameGrid.CenterY)
            {
                return X >= gameGrid.CenterX ? Quarter.TopRight : Quarter.TopLeft;
            }
            else
            {
                return X >= gameGrid.CenterX ? Quarter.BottomRight : Quarter.BottomLeft;
            }
        }
    }

    void Awake()
    {
        _targetPosition = transform.position;
        _spriteRenderer = transform.Find(_frameName).gameObject.GetComponent<SpriteRenderer>();
        Unit.TriggerDeath += Delete;
    }

    void Start()
    {
        SetColorByConnection();
        OnSelection += SetColorBySelection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp( transform.position, _targetPosition,
                                           Time.deltaTime * GameGrid.Instance.RotationSpeed );
    }

    public void InitPosition( uint x, uint y )
    {
        X = x;
        Y = y;

        GameGrid gameGrid = GameGrid.Instance;
        Vector3 position = new Vector3
            {
                x = ( X + 0.5f ) * gameGrid.CellSize,
                y = ( Y + 0.5f ) * gameGrid.CellSize,
                z = gameGrid.FixedZ
            };
        transform.position = position;
        _targetPosition = position;
    }

    public void SetPosition( uint x, uint y )
    {
        X = x;
        Y = y;

        GameGrid gameGrid = GameGrid.Instance;
        _targetPosition.x = ( X + 0.5f ) * gameGrid.CellSize;
        _targetPosition.y = ( Y + 0.5f ) * gameGrid.CellSize;
        _targetPosition.z = gameGrid.FixedZ;
    }

    void SetConnected( bool value )
    {
        if ( _connected == value )
            return;
        _connected = value;

        SetColorByConnection();
    }

    void SetColorByConnection()
    {
        if (!_spriteRenderer || Selected)
            return;

        _spriteRenderer.color = Connected ? Color.white : Color.grey;
    }

    void SetColorBySelection( bool value )
    {
        if (!_spriteRenderer)
            return;

        if (value)
            _spriteRenderer.color = Color.green;
        else
            SetColorByConnection();
    }

    void SelectCallback()
    {
        if ( _selected )
            return;

        _selected = true;
        if ( OnSelection != null )
            OnSelection.Invoke( true );
    }

    void UnselectCallback()
    {
        if ( !_selected )
            return;

        _selected = false;

        if ( OnSelection != null )
            OnSelection.Invoke( false );
    }

    public bool IsConnectedTo( GameCell cell )
    {
        if ( cell == null || cell == this )
            return false;

        int x1 = (int)X;
        int x2 = (int)cell.X;

        int y1 = (int)Y;
        int y2 = (int)cell.Y;

        if ( X == cell.X )
        {
            if ( Math.Abs( y1 - y2 ) == 1 )
                return true;
        }
        else if ( Y == cell.Y )
        {
            if ( Math.Abs( x1 - x2 ) == 1 )
                return true;
        }
        return false;
    }

    public void LaunchAction( Vector3 target )
    {
        if ( OnActionLaunched != null )
            OnActionLaunched.Invoke( target );
    }

    public void LaunchDraggedAction(Vector3 target)
    {
        if (OnDraggedActionLaunched != null)
            OnDraggedActionLaunched.Invoke(target);
    }

    void Delete()
    {
        GameGrid.Instance.RemoveCase( X, Y );
    }
}