using System;
using Orbit.Entity;
using UnityEngine;

public class GameCell : MonoBehaviour
{
    public delegate void DelegateBool( bool value );

    public delegate void DelegateVector3( Vector3 value );

    public enum Quarter
    {
        TopRight
        , BottomRight
        , BottomLeft
        , TopLeft
    }

    private bool _connected;

    private readonly string _frameName = "Frame_Cell";
    private bool _selected;

    private SpriteRenderer _spriteRenderer;

    private Vector3 _targetPosition;

    private AUnitController _unit;

    public DelegateVector3 OnActionLaunched;
    public DelegateVector3 OnDraggedActionLaunched;

    public uint X { get; private set; }
    public uint Y { get; private set; }

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
    public AUnitController Unit
    {
        get
        {
            if ( _unit == null )
                _unit = GetComponent<AUnitController>();
            return _unit;
        }
    }

    public Quarter QuarterPosition
    {
        get
        {
            GameGrid gameGrid = GameGrid.Instance;
            if ( Y >= gameGrid.CenterY )
                return X >= gameGrid.CenterX ? Quarter.TopRight : Quarter.TopLeft;
            return X >= gameGrid.CenterX ? Quarter.BottomRight : Quarter.BottomLeft;
        }
    }

    public event DelegateBool OnSelection;

    private void Awake()
    {
        _targetPosition = transform.position;
        _spriteRenderer = transform.Find( _frameName ).gameObject.GetComponent<SpriteRenderer>();
        Unit.TriggerDeath += Delete;
    }

    private void Start()
    {
        SetColorByConnection();
        OnSelection += SetColorBySelection;
    }

    // Update is called once per frame
    private void Update()
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
                x = ( X + 0.5f ) * gameGrid.CellSize
                , y = ( Y + 0.5f ) * gameGrid.CellSize
                , z = gameGrid.FixedZ
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

    private void SetConnected( bool value )
    {
        if ( _connected == value )
            return;
        _connected = value;

        SetColorByConnection();
    }

    private void SetColorByConnection()
    {
        if ( !_spriteRenderer || Selected )
            return;

        _spriteRenderer.color = Connected ? Color.white : Color.grey;
    }

    private void SetColorBySelection( bool value )
    {
        if ( !_spriteRenderer )
            return;

        if ( value )
            _spriteRenderer.color = Color.green;
        else
            SetColorByConnection();
    }

    private void SelectCallback()
    {
        if ( _selected )
            return;

        _selected = true;
        if ( OnSelection != null )
            OnSelection.Invoke( true );
    }

    private void UnselectCallback()
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

        int x1 = ( int )X;
        int x2 = ( int )cell.X;

        int y1 = ( int )Y;
        int y2 = ( int )cell.Y;

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

    public void LaunchDraggedAction( Vector3 target )
    {
        if ( OnDraggedActionLaunched != null )
            OnDraggedActionLaunched.Invoke( target );
    }

    private void Delete()
    {
        GameGrid.Instance.RemoveCase( X, Y );
    }
}