using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUiScript : MonoBehaviour
{

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Text _resourcesText;

    // Use this for initialization
    void Start () {
        _playButton.onClick.AddListener(OnPlayClicked);
        GameManager.Instance.OnResourcesChange += OnResourceChange;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnResourcesChange -= OnResourceChange;
    }

    void OnPlayClicked()
    {
        GameManager.Instance.CurrentGameMode = GameManager.GameMode.Attacking;
    }

    void OnResourceChange( uint count )
    {
        _resourcesText.text = count.ToString();
    }
}
