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
    private uint _side;

    public uint Side
    {
        get { return _side; }
        private set { _side = value; }
    }

    public float RealSide { get { return Side * CellSize; } }

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
    public float RealEfficientSide {  get { return EfficientSide * CellSize; } }

    public uint CenterX { get; private set; }
    public uint CenterY { get; private set; }

    public Vector3 RealCenter
    {
        get { return new Vector3(CenterX * CellSize, CenterY * CellSize, FixedZ); }
    }

    public uint PosX { get; private set; }
    public uint PosY { get; private set; }

    public Vector3 RealPosition
    {
        get { return new Vector3(PosX * CellSize, PosY * CellSize, FixedZ); }
    }

    public UnityEvent OnLayoutChanged =  new UnityEvent();
    public UnityEvent OnGridEmpty = new UnityEvent();

    private GameCell[,] _grid;

    private uint _cellCount = 0;

    public float FixedZ { get; private set; }

    void Awake()
    {
        FixedZ = transform.position.z;
        transform.position = new Vector3(0, 0, FixedZ);
        _grid = new GameCell[Side, Side];
        CheckGrid();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for ( int x = 0; x < Side; ++x )
        {
            Vector3 p1 = new Vector3(x * CellSize, 0, FixedZ);
            Vector3 p2 = new Vector3(x * CellSize, RealSide, FixedZ);
            Gizmos.DrawLine(p1, p2);
        }
        for (int y = 0; y < Side; ++y)
        {
            Vector3 p1 = new Vector3(0, y * CellSize, FixedZ);
            Vector3 p2 = new Vector3(RealSide, y * CellSize, FixedZ);
            Gizmos.DrawLine(p1, p2);
        }

        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Vector3 position = RealCenter;
        Vector3 size = new Vector3(RealEfficientSide, RealEfficientSide, 1);
        Gizmos.DrawCube(position, size);

        //Draw CenterX, CenterY
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(position, 1.0f);

        //Draw PosX and PosY
        Gizmos.color = Color.magenta;
        position = RealPosition;
        Gizmos.DrawSphere(position, 1.0f);

    }

    void InitCellPosition( GameCell cell, uint x, uint y )
    {
        _grid[x, y] = cell;
        if (!cell)
            return;

        cell.InitPosition(x, y);
    }

    void SetCellPosition(GameCell cell, uint x, uint y)
    {
        _grid[x, y] = cell;
        if (!cell)
            return;

        cell.SetPosition( x, y);
    }

    public void AddCase (GameCell cell, uint x, uint y )
    {
        if (!cell)
            return;

        //if (IsConnected(x, y))
        {
            GameCell createdCell = Instantiate( cell );
            InitCellPosition(createdCell, x, y);

            CheckGrid();

            if (OnLayoutChanged != null)
                OnLayoutChanged.Invoke();
        }
    }

    public bool IsConnected(uint x, uint y)
    {
        if (!_grid[x, y])
            return false;

        bool result = false;

        if ( x < Side - 1)
            result = _grid[x + 1, y] != null;
        if ( x > 1 )
            result = _grid[x - 1, y] != null || result;
        if ( y < Side - 1)
            result = _grid[x, y + 1] != null || result;
        if (y > 1)
            result = _grid[x, y - 1] != null || result;

        return result;
    }

    public void RemoveCase ( uint x, uint y )
    {
        if (_grid[x, y])
        {
            Destroy(_grid[x, y]);
            _grid[x, y] = null;

            CheckGrid();

            if (OnLayoutChanged != null)
                OnLayoutChanged.Invoke();

        }
    }

    public void CheckGrid()
    {
        _cellCount = 0;
        uint bottomLeftX = Side, bottomLeftY = Side, topRightX = 0, topRightY = 0;
        for (uint x = 0; x < Side; ++x)
        {
            for (uint y = 0; y < Side; ++y)
            {
                if (!_grid[x, y]) continue;

                bottomLeftX = x < bottomLeftX ? x : bottomLeftX;
                bottomLeftY = y < bottomLeftY ? y : bottomLeftY;

                topRightX = x + 1 > topRightX ? x + 1 : topRightX;
                topRightY = y + 1 > topRightY ? y + 1 : topRightY;

                _grid[x, y].Connected = IsConnected(x, y);   

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
        uint u = EfficientSide / 2;

        for (uint x = 0; x < u; ++x)
        {
            for (uint y = 0; y < u; ++y)
            {
                GameCell tmpCell = _grid[x + CenterX, y + CenterY];

                SetCellPosition(_grid[CenterX - x, y + CenterY], x + CenterX, y + CenterY);
                SetCellPosition(_grid[CenterX - x, CenterY - y], CenterX - x, CenterY + y);
                SetCellPosition(_grid[x + CenterX, CenterY - y], CenterX - x, CenterY - y);
                SetCellPosition(tmpCell, x + CenterX, CenterY - y);
            }
        }

        CheckGrid();
    }

    public void RotateReverseClockwise()
    {
        uint u = EfficientSide / 2;

        for (uint x = 0; x < u; ++x)
        {
            for (uint y = 0; y < u; ++y)
            {
                GameCell tmpCell = _grid[x + CenterX, y + CenterY];

                SetCellPosition(_grid[x + CenterX, CenterY - y], CenterX + x, CenterY + y);
                SetCellPosition(_grid[CenterX - x, CenterY - y], CenterX + x, CenterY - y);
                SetCellPosition(_grid[CenterX - x, y + CenterY], CenterX - x, CenterY - y);
                SetCellPosition(tmpCell, CenterX - x, CenterY + y);
            }
        }
        CheckGrid();
    }

    public GameCell GetCellFromWorldPoint( Vector3 point )
    {
        int posX = (int)(point.x / CellSize);
        int posY = (int)(point.y / CellSize);

        if ( point.z != FixedZ )
            Debug.Log( "Z does not correspond" );
        if (posX > 0 && posX < Side)
            if (posY > 0 && posY < Side)
                if (_grid[posX, posY])
                    return _grid[posX, posY];

        return null;
    }

    public bool GetPositionFromWorldPoint(Vector3 point, out int x, out int y)
    {
        int posX = (int)(point.x / CellSize);
        int posY = (int)(point.y / CellSize);

        if (point.z != FixedZ)
            Debug.Log("Z does not correspond");

        x = -1;
        y = -1;

        if (posX > 0 && posX < Side)
            if ( posY > 0 && posY < Side )
            {
                x = posX;
                y = posY;
                return true;
            }

        return false;
    }
}
