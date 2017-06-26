using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private static GuiManager _instance;

    public static GuiManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<GuiManager>();
            return _instance;
        }
    }

    [SerializeField]
    private GameObject _hudPrefab;
    private GameObject _hudObject;
    [SerializeField]
    private GameObject _buildUiPrefab;
    private GameObject _buildUiObject;
    [SerializeField]
    private GameObject _pauseUiPrefab;
    private GameObject _pauseUiObject;
    [SerializeField]
    private GameObject _gameOverUiPrefab;
    private GameObject _gameOverUiObject;

    // Use this for initialization
    void Awake()
    {
		GameManager.Instance.OnPlay.AddListener( CleanUi );
        GameManager.Instance.OnPause.AddListener( ShowPauseUi );
        GameManager.Instance.OnGameOver.AddListener( ShowGameOverUi );

        GameManager.Instance.OnAttackMode.AddListener( ShowHud );
        GameManager.Instance.OnBuildMode.AddListener( ShowBuildUi );
    }

    void CleanUi()
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

    void CleanHuds()
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

    void ShowHud()
    {
        CleanHuds();
        if ( _hudPrefab && _hudObject == null )
            _hudObject = Instantiate( _hudPrefab, transform, false );
    }

    void ShowPauseUi()
    {
        CleanUi();
        if ( _pauseUiPrefab && _pauseUiObject == null )
            _pauseUiObject = Instantiate( _pauseUiPrefab, transform, false );
    }

    void ShowGameOverUi()
    {
        CleanUi();
        if ( _gameOverUiPrefab && _gameOverUiObject == null )
            _gameOverUiObject = Instantiate( _gameOverUiPrefab, transform, false );
    }

    void ShowBuildUi()
    {
        CleanHuds();
        if ( _buildUiPrefab && _buildUiObject == null )
            _buildUiObject = Instantiate( _buildUiPrefab, transform, false );
    }
		
}