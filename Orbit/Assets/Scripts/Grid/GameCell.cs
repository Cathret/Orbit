using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameCell : MonoBehaviour
{
    private static GameCell _lastSelected;

    private uint X;
    private uint Y;

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

    public UnityEvent OnSelection;
    public UnityEvent OnUnselection;

    public bool Connected
    {
        get { return _connected; }
        set { SetConnected(value); }
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

        _targetPosition.x = (x + 0.5f) * Grid.CellSize;
        _targetPosition.y = (y + 0.5f) * Grid.CellSize;
        _targetPosition.z = Grid.FixedX;
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
            OnSelection.Invoke();
    }

    void Unselect()
    {
        _selected = false;
        _lastSelected = null;
        if (OnUnselection != null)
            OnUnselection.Invoke();
    }
}
