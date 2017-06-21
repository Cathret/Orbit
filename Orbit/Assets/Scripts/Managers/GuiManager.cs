using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
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

    private Transform _canvas;
    // Use this for initialization
    void Start ()
    {
        _canvas = GetComponent<Canvas>().transform;

        GameManager.Instance.OnPlay.AddListener(ShowHud);
        GameManager.Instance.OnPause.AddListener(ShowPauseUi);
        GameManager.Instance.OnGameOver.AddListener(ShowGameOverUi);
        GameManager.Instance.OnBuildSetEnabled += ShowBuildUi;
    }

    void CleanCanvas()
    {
        if ( _hudObject )
        {
            Destroy( _hudObject );
            _hudObject = null;
        }
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

    void ShowHud()
    {
        CleanCanvas();
        if (_hudPrefab && _hudObject )
            _hudObject = Instantiate( _hudPrefab, _canvas, false );
    }

    void ShowPauseUi()
    {
        CleanCanvas();
        if (_pauseUiPrefab && _pauseUiObject )
            _pauseUiObject = Instantiate(_pauseUiPrefab, _canvas, false);
    }

    void ShowGameOverUi()
    {
        CleanCanvas();
        if (_gameOverUiPrefab && _gameOverUiObject)
            _gameOverUiObject = Instantiate(_gameOverUiPrefab, _canvas, false);
    }

    void ShowBuildUi( bool show )
    {
        if ( show )
        {
            if ( _buildUiPrefab && _buildUiObject == null )
                _buildUiObject = Instantiate(_buildUiPrefab, _canvas, false );
        }
        else
        {
            if ( _buildUiObject )
            {
                Destroy(_buildUiObject);
                _buildUiObject = null;
            }
        }
    }
}
