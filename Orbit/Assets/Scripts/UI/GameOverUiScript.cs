using UnityEngine;
using UnityEngine.UI;

public class GameOverUiScript : MonoBehaviour
{
    [SerializeField]
    private Button _quitButton;
    [SerializeField]
    private Button _replayButton;

    // Use this for initialization
    private void Start()
    {
        _replayButton.onClick.AddListener( GameManager.Instance.Restart );
        _quitButton.onClick.AddListener( GameManager.Instance.ToMainMenu );
    }
}