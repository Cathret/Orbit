using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUiScript : MonoBehaviour {

    [SerializeField]
    private Button _replayButton;

    [SerializeField]
    private Button _quitButton;

    // Use this for initialization
    void Start ()
    {
        _replayButton.onClick.AddListener( GameManager.Instance.Restart );
        _quitButton.onClick.AddListener(GameManager.Instance.ToMainMenu);
    }
	
}
