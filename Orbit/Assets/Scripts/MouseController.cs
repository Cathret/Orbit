using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private static MouseController _instance;

    private readonly List<GameCell> _selectedCells = new List<GameCell>();

    private InsertionMenu _currentInsertionMenu;

    private ManagementMenu _currentManageMenu;

    private GameObject _highlight;

    [SerializeField]
    private GameObject _highlightPrefab;

    [SerializeField]
    private InsertionMenu _insertionMenu;

    [SerializeField]
    private ManagementMenu _manageMenu;

    public static MouseController Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<MouseController>();
            return _instance;
        }
    }

    private void Awake()
    {
        GameManager.Instance.OnBuildMode.AddListener( SwitchToBuildMode );
    }

    private void Start()
    {
        _highlight = Instantiate( _highlightPrefab );

        MiniGestureRecognizer.Swipe += HandleSwipe;
        MiniGestureRecognizer.Click += HandleClick;
        MiniGestureRecognizer.OnDragStart += HandleDrag;
        MiniGestureRecognizer.OnDrag += HandleDragging;
        MiniGestureRecognizer.OnDrop += HandleDrop;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMouse();
    }

    private void HandleMouse()
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleMouseInAttackMode();
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleMouseInBuildMode();
    }

    private void HandleClick( Vector2 position )
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleClickInAttackMode( position );
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleClickInBuildMode( position );
    }

    private void HandleDrag( Vector2 position )
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleDragInAttackMode( position );
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleDragInBuildMode( position );
    }

    private void HandleDragging( Vector2 position )
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleDraggingInAttackMode( position );
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleDraggingInBuildMode( position );
    }

    private void HandleDrop( Vector2 position )
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleDropInAttackMode( position );
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleDropInBuildMode( position );
    }

    private void HandleSwipe( MiniGestureRecognizer.SwipeDirection direction )
    {
        if ( !GameManager.Instance.Playing )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleSwipeInAttackMode( direction );
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleSwipeInBuildMode( direction );
    }

    private void DraggedAction( Vector2 mousePos )
    {
        Vector3 pos = MouseWorldPosition( mousePos );
        foreach ( GameCell cell in _selectedCells )
            if ( cell && cell.Connected )
                cell.LaunchDraggedAction( pos );
    }

    private void Action( Vector2 mousePos )
    {
        Vector3 pos = MouseWorldPosition( mousePos );
        foreach ( GameCell cell in _selectedCells )
            if ( cell && cell.Connected )
                cell.LaunchAction( pos );
    }

    private void ManageCell( GameCell cell )
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

    private void AddCell( Vector3 mouseWorldPos )
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

    private void HighlightBuildMode()
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

    private void HighlightCellSelection()
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

    private Vector3 MouseWorldPosition( Vector2 mousePos )
    {
        return Camera.main.ScreenToWorldPoint( new Vector3( mousePos.x, mousePos.y
                                                            , -Camera.main.transform.position.z ) );
    }

    private Vector3 MouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint( new Vector3( mousePos.x, mousePos.y
                                                            , -Camera.main.transform.position.z ) );
    }

    private GameCell PickCell()
    {
        Vector3 pos = MouseWorldPosition();
        return GameGrid.Instance.GetCellFromWorldPoint( pos );
    }

    private GameCell PickCell( Vector2 mousePos )
    {
        Vector3 pos = MouseWorldPosition( mousePos );
        return GameGrid.Instance.GetCellFromWorldPoint( pos );
    }

    private void SwitchToBuildMode()
    {
        UnselectCells();
    }

    private void SelectCell( GameCell cell )
    {
        if ( cell && _selectedCells.Contains( cell ) == false )
        {
            if ( _selectedCells.Count > 0 && cell.Unit.GetType() != _selectedCells[0].Unit.GetType() )
                UnselectCells();
            cell.Selected = true;
            _selectedCells.Add( cell );
        }
    }

    private void UnselectCells()
    {
        foreach ( GameCell cell in _selectedCells )
            cell.Selected = false;
        _selectedCells.Clear();
    }

    #region Attack
    private void HandleClickInAttackMode( Vector2 position )
    {
        GameCell cell = PickCell( position );
        if ( cell == null )
            UnselectCells();
        else
            SelectCell( cell );
    }

    private void HandleDragInAttackMode( Vector2 position )
    {
        GameCell cell = PickCell( position );
        if ( cell )
            if ( !_selectedCells.Contains( cell ) )
            {
                UnselectCells();
                SelectCell( cell );
            }
    }

    private void HandleDraggingInAttackMode( Vector2 position )
    {
        DraggedAction( position );
    }

    private void HandleDropInAttackMode( Vector2 position )
    {
        Action( position );
    }

    private void HandleSwipeInAttackMode( MiniGestureRecognizer.SwipeDirection direction )
    {
        if ( direction == MiniGestureRecognizer.SwipeDirection.Right )
            GameGrid.Instance.RotateClockwise();
        else if ( direction == MiniGestureRecognizer.SwipeDirection.Left )
            GameGrid.Instance.RotateReverseClockwise();
    }

    private void HandleMouseInAttackMode()
    {
        HighlightCellSelection();
    }
    #endregion

    #region Build
    private void HandleClickInBuildMode( Vector2 position )
    {
        if ( _currentInsertionMenu || _currentManageMenu )
            return;

        Vector3 pos = MouseWorldPosition();

        GameCell cell = GameGrid.Instance.GetCellFromWorldPoint( pos );

        if ( cell )
            ManageCell( cell );
        else
            AddCell( pos );
    }

    private void HandleDragInBuildMode( Vector2 position )
    {
    }

    private void HandleDraggingInBuildMode( Vector2 position )
    {
    }

    private void HandleDropInBuildMode( Vector2 position )
    {
    }

    private void HandleSwipeInBuildMode( MiniGestureRecognizer.SwipeDirection direction )
    {
    }

    private void HandleMouseInBuildMode()
    {
        if ( _currentInsertionMenu || _currentManageMenu )
            return;

        HighlightBuildMode();
    }
    #endregion
}