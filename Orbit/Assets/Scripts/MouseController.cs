using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private static MouseController _instance;

    public static MouseController Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<MouseController>();
            return _instance;
        }
    }

    [SerializeField]
    private GameObject _highlightPrefab;

    private GameObject _highlight;

    [SerializeField]
    private InsertionMenu _insertionMenu;

    private InsertionMenu _currentInsertionMenu;

    [SerializeField]
    private ManagementMenu _manageMenu;

    private ManagementMenu _currentManageMenu;

    private readonly List<GameCell> _selectedCells = new List<GameCell>();

    void Awake()
    {
        GameManager.Instance.OnBuildMode.AddListener( SwitchToBuildMode );
    }

    void Start()
    {
        _highlight = Instantiate( _highlightPrefab );

        MiniGestureRecognizer.Swipe += HandleSwipe;
        MiniGestureRecognizer.Click += HandleClick;
        MiniGestureRecognizer.OnDragStart += HandleDrag;
        MiniGestureRecognizer.OnDrag += HandleDragging;
        MiniGestureRecognizer.OnDrop += HandleDrop;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMouse();
    }

    void HandleMouse()
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleMouseInAttackMode();
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleMouseInBuildMode();
    }

    void HandleClick(Vector2 position)
    {
        if (!GameManager.Instance.Playing)
            return;

        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking)
            HandleClickInAttackMode(position);
        else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building)
            HandleClickInBuildMode(position);
    }

    void HandleDrag(Vector2 position)
    {
        if (!GameManager.Instance.Playing)
            return;

        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking)
            HandleDragInAttackMode(position);
        else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building)
            HandleDragInBuildMode(position);
    }

    void HandleDragging(Vector2 position)
    {
        if (!GameManager.Instance.Playing)
            return;

        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking)
            HandleDraggingInAttackMode(position);
        else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building)
            HandleDraggingInBuildMode(position);
    }

    void HandleDrop(Vector2 position)
    {
        if (!GameManager.Instance.Playing)
            return;

        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking)
            HandleDropInAttackMode(position);
        else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building)
            HandleDropInBuildMode(position);
    }

    void HandleSwipe(MiniGestureRecognizer.SwipeDirection direction)
    {
        if (!GameManager.Instance.Playing)
            return;

        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking)
            HandleSwipeInAttackMode(direction);
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleSwipeInBuildMode( direction );
    }

    #region Attack
    void HandleClickInAttackMode(Vector2 position)
    {
        GameCell cell = PickCell(position);
        if (cell == null)
            UnselectCells();
        else
            SelectCell(cell);
    }
    void HandleDragInAttackMode(Vector2 position)
    {
        GameCell cell = PickCell(position);
        if ( cell )
            if ( !_selectedCells.Contains( cell ) )
            {
                UnselectCells();
                SelectCell( cell );
            }
    }
    void HandleDraggingInAttackMode(Vector2 position)
    {
        DraggedAction( position );
    }
    void HandleDropInAttackMode(Vector2 position)
    {
        Action(position);
    }
    void HandleSwipeInAttackMode(MiniGestureRecognizer.SwipeDirection direction)
    {
        if ( direction == MiniGestureRecognizer.SwipeDirection.Right)
            GameGrid.Instance.RotateClockwise();
        else if (direction == MiniGestureRecognizer.SwipeDirection.Left)
            GameGrid.Instance.RotateReverseClockwise();
    }
    void HandleMouseInAttackMode()
    {
        HighlightCellSelection();
    }
    #endregion

    #region Build
    void HandleClickInBuildMode(Vector2 position)
    {
        if (_currentInsertionMenu || _currentManageMenu)
            return;

        Vector3 pos = MouseWorldPosition();

        GameCell cell = GameGrid.Instance.GetCellFromWorldPoint(pos);

        if (cell)
            ManageCell(cell);
        else
            AddCell(pos);
    }
    void HandleDragInBuildMode(Vector2 position)
    {
    }
    void HandleDraggingInBuildMode(Vector2 position)
    {
    }
    void HandleDropInBuildMode(Vector2 position)
    {
    }
    void HandleSwipeInBuildMode(MiniGestureRecognizer.SwipeDirection direction)
    {
    }
    void HandleMouseInBuildMode()
    {
        if ( _currentInsertionMenu || _currentManageMenu )
            return;

        HighlightBuildMode();
    }
#endregion

    void DraggedAction(Vector2 mousePos)
    {
        Vector3 pos = MouseWorldPosition(mousePos);
        foreach (GameCell cell in _selectedCells)
        {
            if (cell && cell.Connected)
                cell.LaunchDraggedAction(pos);
        }
    }

    void Action(Vector2 mousePos)
    {
        Vector3 pos = MouseWorldPosition(mousePos);
        foreach ( GameCell cell in _selectedCells )
        {
            if (cell && cell.Connected)
                cell.LaunchAction(pos);
        }
    }

    void ManageCell( GameCell cell )
    {
        if ( cell )
        {
            _currentManageMenu = Instantiate( _manageMenu, GuiManager.Instance.transform, false );
            _currentManageMenu.transform.position =
                Camera.main.WorldToScreenPoint( GameGrid.Instance.GetRealPosition( cell.X, cell.Y ) );
            _currentManageMenu.DestroyCallback += () => { _currentManageMenu = null; };
            _currentManageMenu.unit = cell.Unit;
        }
    }

    void AddCell( Vector3 mouseWorldPos )
    {
        int x, y;
        if ( GameGrid.Instance.GetPositionFromWorldPoint( mouseWorldPos, out x, out y ) )
        {
            uint ux = ( uint )x;
            uint uy = ( uint )y;
            if ( !GameGrid.Instance.CanBeAdded( ux, uy ) )
                return;
            _currentInsertionMenu = Instantiate( _insertionMenu, GuiManager.Instance.transform, false );
            //_currentInsertionMenu.transform.position = Camera.main.WorldToScreenPoint( GameGrid.Instance.GetRealPosition( ux, uy ) );
            _currentInsertionMenu.X = ux;
            _currentInsertionMenu.Y = uy;
            _currentInsertionMenu.DestroyCallback += () => { _currentInsertionMenu = null; };
        }
    }

    void HighlightBuildMode()
    {
        if ( !_highlight )
            return;

        GameGrid gameGrid = GameGrid.Instance;

        float cellSize = gameGrid.CellSize;

        Vector3 pos = MouseWorldPosition();

        int posX, posY;
        GameGrid.Instance.GetPositionFromWorldPoint( pos, out posX, out posY );

        if ( posX == -1 || posY == -1 )
            return;

        _highlight.SetActive( GameGrid.Instance.CanHighlightBuildMode( ( uint )posX, ( uint )posY ) );

        _highlight.transform.position = new Vector3( ( posX + 0.5f ) * cellSize,
                                                     ( posY + 0.5f ) * cellSize,
                                                     gameGrid.FixedZ );
    }

    void HighlightCellSelection()
    {
        if ( !_highlight )
            return;

        GameGrid gameGrid = GameGrid.Instance;

        float cellSize = gameGrid.CellSize;

        GameCell cell = PickCell();

        if ( cell )
        {
            uint posX = cell.X;
            uint posY = cell.Y;
            _highlight.SetActive( GameGrid.Instance.GetCell( posX, posY ) != null );

            _highlight.transform.position = new Vector3( ( posX + 0.5f ) * cellSize,
                                                         ( posY + 0.5f ) * cellSize,
                                                         gameGrid.FixedZ );
        }
    }

    Vector3 MouseWorldPosition( Vector2 mousePos )
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y
                                                          , -Camera.main.transform.position.z));
    }

    Vector3 MouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y
                                                       , -Camera.main.transform.position.z));
    }

    GameCell PickCell()
    {
        Vector3 pos = MouseWorldPosition();
        return GameGrid.Instance.GetCellFromWorldPoint(pos);
    }

    GameCell PickCell( Vector2 mousePos )
    {
        Vector3 pos = MouseWorldPosition(mousePos);
        return GameGrid.Instance.GetCellFromWorldPoint(pos);
    }

    void SwitchToBuildMode()
    {
        UnselectCells();
    }

    void SelectCell(GameCell cell)
    {
        if (cell && _selectedCells.Contains(cell) == false)
        {
            if ( _selectedCells.Count > 0 && cell.GetType() != _selectedCells[0].GetType() )
                return;
            cell.Selected = true;
            _selectedCells.Add(cell);
        }
    }

    void UnselectCells()
    {
        foreach ( GameCell cell in _selectedCells )
        {
            cell.Selected = false;
        }
        _selectedCells.Clear();
    }
}