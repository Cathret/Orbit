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

    void Awake()
    {
        GameManager.Instance.OnBuildMode.AddListener( SwitchToBuildMode );
    }

    void Start()
    {
        _highlight = Instantiate( _highlightPrefab );
    }

    // Update is called once per frame
    void Update()
    {
        if ( !_currentInsertionMenu )
            HandleMouse();
    }

    void HandleMouse()
    {
        if ( GameManager.Instance.CurrentGameState != GameManager.GameState.Play )
            return;

        if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Attacking )
            HandleMouseInAttackMode();
        else if ( GameManager.Instance.CurrentGameMode == GameManager.GameMode.Building )
            HandleMouseInBuildMode();
    }

    void HandleMouseInBuildMode()
    {
        if ( _currentInsertionMenu || _currentManageMenu )
            return;

        HighlightBuildMode();

        Vector3 pos = MouseWorldPosition();

        GameCell cell = GameGrid.Instance.GetCellFromWorldPoint( pos );

        if ( Input.GetMouseButtonDown( 0 ) )
            if ( cell )
                ManageCell( cell );
            else
                AddCell( pos );
    }

    void HandleMouseInAttackMode()
    {
        HighlightCellSelection();

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            GameCell cell = PickCell();
            if ( GameCell.SelectedCell != null && cell == null )
                Action();
            else
                SelectCell( cell );
        }
    }

    void Action()
    {
        GameCell cell = GameCell.SelectedCell;
        if ( cell && cell.Connected )
        {
            Vector3 pos = MouseWorldPosition();
            cell.LaunchAction( pos );
        }
    }

    void SelectCell( GameCell cell )
    {
        if ( cell )
            if ( cell.Connected )
                cell.Selected = true;
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

    void SwitchToBuildMode()
    {
        GameCell.Unselect();
    }
}