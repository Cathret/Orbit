using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUiScript : MonoBehaviour {

    [SerializeField]
    private Button _continueButton;

    [SerializeField]
    private Button _quitButton;

    // Use this for initialization
    void Start ()
    {
        _continueButton.onClick.AddListener( Continue );
        _quitButton.onClick.AddListener(GameManager.Instance.ToMainMenu);
    }

    void Continue()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.Play;
    }
}
