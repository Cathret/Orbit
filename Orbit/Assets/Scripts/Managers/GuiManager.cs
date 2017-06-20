using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _hudPrefab;
    [SerializeField]
    private GameObject _pauseUiPrefab;
    [SerializeField]
    private GameObject _gameOverUiPrefab;

    private Transform _canvas;
    // Use this for initialization
    void Start ()
    {
        _canvas = FindObjectOfType<Canvas>().transform;

        GameManager.Instance.OnPlay.AddListener(ShowHUD);
        GameManager.Instance.OnPlay.AddListener(ShowPauseUI);
        GameManager.Instance.OnPlay.AddListener(ShowGameOverUI);
    }

    void CleanCanvas()
    {
        if (_canvas)
            _canvas.DetachChildren();
    }

    void ShowHUD()
    {
        CleanCanvas();
    }

    void ShowPauseUI()
    {
        CleanCanvas();
    }

    void ShowGameOverUI()
    {
        CleanCanvas();
    }
}
