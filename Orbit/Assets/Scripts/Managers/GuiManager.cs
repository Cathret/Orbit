using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private static GuiManager _instance;
    private GameObject _buildUiObject;
    [SerializeField]
    private GameObject _buildUiPrefab;
    private GameObject _gameOverUiObject;
    [SerializeField]
    private GameObject _gameOverUiPrefab;
    private GameObject _hudObject;

    [SerializeField]
    private GameObject _hudPrefab;
    private GameObject _pauseUiObject;
    [SerializeField]
    private GameObject _pauseUiPrefab;

    public static GuiManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<GuiManager>();
            return _instance;
        }
    }

    // Use this for initialization
    private void Awake()
    {
        GameManager.Instance.OnPlay.AddListener( CleanUi );
        GameManager.Instance.OnPause.AddListener( ShowPauseUi );
        GameManager.Instance.OnGameOver.AddListener( ShowGameOverUi );

        GameManager.Instance.OnAttackMode.AddListener( ShowHud );
        GameManager.Instance.OnBuildMode.AddListener( ShowBuildUi );
    }

    private void CleanUi()
    {
        if ( _pauseUiObject )
        {
            Destroy( _pauseUiObject );
            _pauseUiObject = null;
        }
        if ( _gameOverUiObject )
        {
            Destroy( _gameOverUiObject );
            _gameOverUiObject = null;
        }
    }

    private void CleanHuds()
    {
        if ( _hudObject )
        {
            Destroy( _hudObject );
            _hudObject = null;
        }
        if ( _buildUiObject )
        {
            Destroy( _buildUiObject );
            _buildUiObject = null;
        }
    }

    private void ShowHud()
    {
        CleanHuds();
        if ( _hudPrefab && _hudObject == null )
            _hudObject = Instantiate( _hudPrefab, transform, false );
    }

    private void ShowPauseUi()
    {
        CleanUi();
        if ( _pauseUiPrefab && _pauseUiObject == null )
            _pauseUiObject = Instantiate( _pauseUiPrefab, transform, false );
    }

    private void ShowGameOverUi()
    {
        CleanUi();
        if ( _gameOverUiPrefab && _gameOverUiObject == null )
            _gameOverUiObject = Instantiate( _gameOverUiPrefab, transform, false );
    }

    private void ShowBuildUi()
    {
        CleanHuds();
        if ( _buildUiPrefab && _buildUiObject == null )
            _buildUiObject = Instantiate( _buildUiPrefab, transform, false );
    }
}