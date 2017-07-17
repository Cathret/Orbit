using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameGrid : MonoBehaviour
{
    private static GameGrid _instance;
    public static GameGrid Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<GameGrid>();
            return _instance;
        }
    }

    [SerializeField]
	private GameCell _defaultCell1;
	[SerializeField]
	private GameCell _defaultCell2;
	[SerializeField]
	private GameCell _defaultCell3;
	[SerializeField]
	private GameCell _defaultCell4;

    [SerializeField]
    private uint _side;

    public uint Side
    {
        get { return _side; }
        private set { _side = value; }
    }

    public float RealSide
    {
        get { return Side * CellSize; }
    }

    [SerializeField]
    private float _cellSize;

    public float CellSize
    {
        get { return _cellSize; }
        private set { _cellSize = value; }
    }

    [SerializeField]
    private float _rotationSpeed = 20.0f;
    public float RotationSpeed
    {
        get { return _rotationSpeed; }
        private set { _rotationSpeed = value; }
    }

    public uint EfficientSide { get; private set; }
    public float RealEfficientSide
    {
        get { return EfficientSide * CellSize; }
    }

    public uint CenterX { get; private set; }
    public uint CenterY { get; private set; }

    public Vector3 RealCenter
    {
        get { return new Vector3( CenterX * CellSize, CenterY * CellSize, FixedZ ); }
    }

    public uint PosX { get; private set; }
    public uint PosY { get; private set; }

    public Vector3 RealPosition
    {
        get { return new Vector3( PosX * CellSize, PosY * CellSize, FixedZ ); }
    }

    public UnityEvent OnLayoutChanged = new UnityEvent();
    public UnityEvent OnGridEmpty = new UnityEvent();

    private GameCell[,] _grid;

    private uint _cellCount = 0;

    public float FixedZ { get; private set; }

    void Awake()
    {
        FixedZ = transform.position.z;
        transform.position = new Vector3( 0, 0, FixedZ );
        _grid = new GameCell[Side, Side];

        CheckGrid();

		uint x = PosX;
		uint y = PosY;
		if (_defaultCell1)
			AddCase (_defaultCell1, x, y, true);
		if (_defaultCell2)
			AddCase (_defaultCell2, x + 1, y, true);
		if (_defaultCell3)
			AddCase (_defaultCell3, x + 1, y - 1, true);
		if (_defaultCell4)
			AddCase (_defaultCell4, x, y - 1 , true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for ( int x = 0; x < Side; ++x )
        {
            Vector3 p1 = new Vector3( x * CellSize, 0, FixedZ );
            Vector3 p2 = new Vector3( x * CellSize, RealSide, FixedZ );
            Gizmos.DrawLine( p1, p2 );
        }
        for ( int y = 0; y < Side; ++y )
        {
            Vector3 p1 = new Vector3( 0, y * CellSize, FixedZ );
            Vector3 p2 = new Vector3( RealSide, y * CellSize, FixedZ );
            Gizmos.DrawLine( p1, p2 );
        }

        Gizmos.color = new Color( 1, 0, 0, 0.5F );
        Vector3 position = RealCenter;
        Vector3 size = new Vector3( RealEfficientSide, RealEfficientSide, 1 );
        Gizmos.DrawCube( position, size );

        //Draw CenterX, CenterY
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere( position, 1.0f );

        //Draw PosX and PosY
        Gizmos.color = Color.magenta;
        position = RealPosition;
        Gizmos.DrawSphere( position, 1.0f );
    }

    void InitCellPosition( GameCell cell, uint x, uint y )
    {
        _grid[x, y] = cell;
        if ( !cell )
            return;

        cell.InitPosition( x, y );
    }

    void SetCellPosition( GameCell cell, uint x, uint y )
    {
        _grid[x, y] = cell;
        if ( !cell )
            return;

        cell.SetPosition( x, y );
    }

	public void AddCase( GameCell cell, uint x, uint y, bool force = false )
    {
        if ( !cell )
            return;

		if (force || CanBeAdded(x, y))
        {
            GameCell createdCell = Instantiate( cell );
            InitCellPosition( createdCell, x, y );

            CheckGrid();

            if ( OnLayoutChanged != null )
                OnLayoutChanged.Invoke();
        }
    }

    public bool CanHighlightBuildMode( uint x, uint y )
    {
        bool result = false;

        if ( x < Side - 1 )
            result = _grid[x + 1, y] != null;
        if ( x > 1 )
            result = _grid[x - 1, y] != null || result;
        if ( y < Side - 1 )
            result = _grid[x, y + 1] != null || result;
        if ( y > 1 )
            result = _grid[x, y - 1] != null || result;

        return ( result || _grid[x, y] != null );
    }

    public bool CanBeAdded( uint x, uint y )
    {
        return CanBeConnected( x, y ) && _grid[x, y] == null;
    }

    public bool CanBeConnected( uint x, uint y )
    {
        bool result = false;

        if ( x < Side - 1 )
            result = _grid[x + 1, y] != null;
        if ( x > 1 )
            result = _grid[x - 1, y] != null || result;
        if ( y < Side - 1 )
            result = _grid[x, y + 1] != null || result;
        if ( y > 1 )
            result = _grid[x, y - 1] != null || result;

        return result;
    }

    public bool IsConnected( uint x, uint y )
    {
        if ( !_grid[x, y] )
            return false;

        return CanBeConnected( x, y );
    }

    public void MoveCell( GameCell cell, uint destX, uint destY )
    {
        if (destX > 0 && destX < Side)
            if (destY > 0 && destY < Side)
            {
                _grid[cell.X, cell.Y] = cell;
                cell.SetPosition( destX, destY );
            }
    }

    public void RemoveCell( GameCell cell )
    {
        uint x = cell.X, y = cell.Y;
        if (x > 0 && x < Side)
            if (y > 0 && y < Side)
            {
                RemoveCase( x, y );
            }
    }

    public void CleanCase( uint x, uint y )
    {
        _grid[x, y] = null;
    }

    public void RemoveCase( uint x, uint y )
    {
        if ( _grid[x, y] )
        {
            Destroy( _grid[x, y].gameObject );
            _grid[x, y] = null;

            CheckGrid();

            if ( OnLayoutChanged != null )
                OnLayoutChanged.Invoke();
        }
    }

    public void CheckGrid()
    {
        _cellCount = 0;
        uint bottomLeftX = Side, bottomLeftY = Side, topRightX = 0, topRightY = 0;
        for ( uint x = 0; x < Side; ++x )
        {
            for ( uint y = 0; y < Side; ++y )
            {
                if ( !_grid[x, y] ) continue;

                bottomLeftX = x < bottomLeftX ? x : bottomLeftX;
                bottomLeftY = y < bottomLeftY ? y : bottomLeftY;

                topRightX = x + 1 > topRightX ? x + 1 : topRightX;
                topRightY = y + 1 > topRightY ? y + 1 : topRightY;

                _grid[x, y].Connected = IsConnected( x, y );

                ++_cellCount;
            }
        }

        if ( _cellCount > 0 )
        {
            CenterX = ( topRightX + bottomLeftX ) / 2;
            CenterY = ( topRightY + bottomLeftY ) / 2;

            PosX = bottomLeftX;
            PosY = bottomLeftY;

            uint efficientSide = topRightX - bottomLeftX;
            efficientSide = topRightY - bottomLeftY > efficientSide ? topRightY - bottomLeftY : efficientSide;

            EfficientSide = efficientSide;
        }
        else
        {
            OnGridEmpty.Invoke();

            CenterX = Side / 2;
            CenterY = CenterX;
            PosX = CenterX;
            PosY = CenterY;

            EfficientSide = 0;
        }
    }

    public void RotateClockwise()
    {
        uint u = ( uint )Mathf.CeilToInt(EfficientSide / 2.0f);
        for ( uint x = 0; x < u; ++x )
        {
            for ( uint y = 0; y < u; ++y )
            {
                GameCell tmpCell = _grid[x + CenterX, y + CenterY];

                SetCellPosition( _grid[CenterX - (y + 1), x + CenterY], x + CenterX, y + CenterY );
                SetCellPosition( _grid[CenterX - (x + 1), CenterY - (y + 1)], CenterX - (y + 1), x + CenterY);
                SetCellPosition( _grid[y + CenterX, CenterY - (x + 1)], CenterX - (x + 1), CenterY - (y + 1) );
                SetCellPosition( tmpCell, y + CenterX, CenterY - (x + 1));
            }
        }

        //CheckGrid();
    }

    public void RotateReverseClockwise()
    {
        uint u = (uint)Mathf.CeilToInt(EfficientSide / 2.0f);

        for ( uint x = 0; x < u; ++x )
        {
            for ( uint y = 0; y < u; ++y )
            {
                GameCell tmpCell = _grid[x + CenterX, y + CenterY];

                SetCellPosition(_grid[y + CenterX, CenterY - (x + 1)], x + CenterX, y + CenterY);
                SetCellPosition(_grid[CenterX - (x + 1), CenterY - (y + 1)], y + CenterX, CenterY - (x + 1));
                SetCellPosition(_grid[CenterX - (y + 1), x + CenterY], CenterX - (x + 1), CenterY - (y + 1));
                SetCellPosition(tmpCell, CenterX - (y + 1), x + CenterY);
            }
        }
        //CheckGrid();
    }

    public GameCell GetCell( uint x, uint y )
    {
        if ( x > 0 && x < Side )
            if ( y > 0 && y < Side )
                if ( _grid[x, y] )
                    return _grid[x, y];

        return null;
    }

    public GameCell GetCellFromWorldPoint( Vector3 point )
    {
        int posX = (int)( point.x / CellSize );
        int posY = (int)( point.y / CellSize );

        if ( point.z != FixedZ )
            Debug.Log( "Z does not correspond" );
        if ( posX > 0 && posX < Side )
            if ( posY > 0 && posY < Side )
                if ( _grid[posX, posY] )
                    return _grid[posX, posY];

        return null;
    }

    public bool GetPositionFromWorldPoint( Vector3 point, out int x, out int y )
    {
        int posX = (int)( point.x / CellSize );
        int posY = (int)( point.y / CellSize );

        if ( point.z != FixedZ )
            Debug.Log( "Z does not correspond" );

        x = -1;
        y = -1;

        if ( posX > 0 && posX < Side )
            if ( posY > 0 && posY < Side )
            {
                x = posX;
                y = posY;
                return true;
            }

        return false;
    }

    public Vector3 GetRealPosition(uint x, uint y)
    {
        if (x > 0 && x < Side)
            if ( y > 0 && y < Side )
            {
                return new Vector3(x * CellSize, y * CellSize, FixedZ);
            }
        return new Vector3();
    }
}