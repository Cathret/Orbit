using UnityEngine;
using UnityEngine.UI;

public class BuildUiScript : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Text _resourcesText;

    // Use this for initialization
    private void Start()
    {
        _playButton.onClick.AddListener( OnPlayClicked );
        GameManager.Instance.OnResourcesChange += OnResourceChange;
        OnResourceChange( GameManager.Instance.ResourcesCount );
    }

    private void OnDestroy()
    {
        if ( GameManager.Instance )
            GameManager.Instance.OnResourcesChange -= OnResourceChange;
    }

    private void OnPlayClicked()
    {
        GameManager.Instance.CurrentGameMode = GameManager.GameMode.Attacking;
    }

    private void OnResourceChange( uint count )
    {
        _resourcesText.text = count.ToString();
    }
}