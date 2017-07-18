using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _creditsGameObject;

    [SerializeField]
    private Button _mainMenuButton;

    [SerializeField]
    private GameObject _mainMenuGameObject;

    [SerializeField]
    private GameObject _optionGameObject;

    private void Start()
    {
        ToMainMenu();
    }

    private void Clear()
    {
        if ( _mainMenuButton )
            _mainMenuButton.gameObject.SetActive( false );
        if ( _mainMenuGameObject )
            _mainMenuGameObject.SetActive( false );
        if ( _optionGameObject )
            _optionGameObject.SetActive( false );
        if ( _creditsGameObject )
            _creditsGameObject.SetActive( false );
    }

    public void ToMainMenu()
    {
        Clear();
        if ( _mainMenuButton )
            _mainMenuButton.gameObject.SetActive( false );
        if ( _mainMenuGameObject )
            _mainMenuGameObject.SetActive( true );
    }

    public void Play()
    {
        SceneManager.LoadScene( 1 );
    }

    public void Credits()
    {
        Clear();
        if ( _mainMenuButton )
            _mainMenuButton.gameObject.SetActive( true );
        if ( _creditsGameObject )
            _creditsGameObject.SetActive( true );
    }

    public void Options()
    {
        Clear();
        if ( _mainMenuButton )
            _mainMenuButton.gameObject.SetActive( true );
        if ( _optionGameObject )
            _optionGameObject.SetActive( true );
    }
}