using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField]
    private uint _side;

    public uint Side
    {
        get { return _side; }
        private set { _side = value; }
    }

    [SerializeField]
    private float _cellSize;

    public float CellSize
    {
        get { return _cellSize; }
        private set { _cellSize = value; }
    }

    public uint EfficientSide { get; private set; }

    public uint CenterX { get; private set; }
    public uint CenterY { get; private set; }

    public uint PosX { get; private set; }
    public uint PosY { get; private set; }

    public delegate void SimpleDelegate();
    public event SimpleDelegate OnLayoutChanged;

    private GameCell[,] _grid;

    void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Use this for initialization
    void Start ()
    {
        _grid = new GameCell[Side, Side];
	}

    public void SetCellPosition(GameCell cell, uint x, uint y)
    {
        _grid[x, y] = cell;
        if (!cell)
            return;

        cell.SetPosition(this, x, y);
        cell.Connected = true;
    }

    //TODO: Check if cell is a prefab
    public void AddCase (GameCell cell, uint x, uint y )
    {
        if (!cell)
            return;

        if (IsConnected(x, y))
        {
            SetCellPosition(cell, x, y);

            if (OnLayoutChanged != null)
                OnLayoutChanged();

            CheckGrid();
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

            if (OnLayoutChanged != null)
                OnLayoutChanged();

            CheckGrid();
        }
    }

    public void CheckGrid()
    {
        uint bottomLeftX = Side, bottomLeftY = Side, topRightX = 0, topRightY = 0;

        for (uint x = 0; x < Side; ++x)
        {
            for (uint y = 0; y < Side; ++y)
            {
                if (!_grid[x, y]) continue;

                bottomLeftX = x < bottomLeftX ? x : bottomLeftX;
                bottomLeftY = y < bottomLeftY ? y : bottomLeftY;

                topRightX = x > topRightX ? x : topRightX;
                topRightY = y > topRightY ? y : topRightY;

                _grid[x, y].Connected = IsConnected(x, y);
            }
        }

        CenterX = (topRightX + bottomLeftX) / 2;
        CenterY = (topRightY + bottomLeftY) / 2;

        PosX = bottomLeftX;
        PosY = bottomLeftY;

        uint efficientSide = topRightX - bottomLeftX;
        efficientSide = topRightY - bottomLeftY > efficientSide ? topRightY - bottomLeftY : efficientSide;

        EfficientSide = efficientSide;

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
    }
}
