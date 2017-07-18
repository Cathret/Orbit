using UnityEngine;
using UnityEngine.UI;

public class PauseUiScript : MonoBehaviour
{
    [SerializeField]
    private Button _continueButton;

    [SerializeField]
    private Button _quitButton;

    // Use this for initialization
    private void Start()
    {
        _continueButton.onClick.AddListener( Continue );
        _quitButton.onClick.AddListener( GameManager.Instance.ToMainMenu );
    }

    private void Continue()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.Play;
    }
}