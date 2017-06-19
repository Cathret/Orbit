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

    // Use this for initialization
    void Start ()
    {
        _grid = new GameCell[Side, Side];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO: Check if cell is a prefab
    public void AddCase ( uint x, uint y, GameCell cell )
    {
        if (IsConnected(x, y))
        {
            _grid[x, y] = cell;
            Vector3 position = new Vector3(x * CellSize, y * CellSize);
            cell.transform.position = position;
            cell.X = x;
            cell.Y = y;
            cell.connected = true;

            if (OnLayoutChanged != null)
                OnLayoutChanged();

            CheckGrid();
        }
    }

    public bool IsConnected(uint x, uint y)
    {
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

                _grid[x, y].connected = IsConnected(x, y);
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
}
