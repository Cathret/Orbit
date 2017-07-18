using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    [SerializeField]
    private Text _resourcesText;
    [SerializeField]
    private Button _rotateClockWise;
    [SerializeField]
    private Button _rotateIInvClockWise;
    [SerializeField]
    private Text _timerText;

    [SerializeField]
    [Header( "Warning" )]
    private Image[] _warningImages;
    [SerializeField]
    private uint _warningShowRepetition = 3;
    [SerializeField]
    private float _warningShowTime = 0.3f;
    [SerializeField]
    private Text _zoneText;

    private void Awake()
    {
        GameManager.Instance.OnResourcesChange += UpdateResourcesText;
        UpdateResourcesText( GameManager.Instance.ResourcesCount );
        WaveManager.Instance.RoundChanged += UpdateRoundText;

        WaveManager.Instance.OnNewWave += ShowWarning;
        for ( int i = 0; i < _warningImages.Length; ++i )
            _warningImages[i].gameObject.SetActive( false );
    }

    // Use this for initialization
    private void Start()
    {
        if ( _rotateClockWise )
            _rotateClockWise.onClick.AddListener( GameGrid.Instance.RotateClockwise );

        if ( _rotateIInvClockWise )
            _rotateIInvClockWise.onClick.AddListener( GameGrid.Instance.RotateReverseClockwise );
    }

    private void OnDestroy()
    {
        if ( GameManager.Instance )
            GameManager.Instance.OnResourcesChange -= UpdateResourcesText;

        if ( WaveManager.Instance )
        {
            WaveManager.Instance.RoundChanged -= UpdateRoundText;
            WaveManager.Instance.OnNewWave -= ShowWarning;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if ( !_timerText )
            return;
        float currentTime = WaveManager.Instance.TimeToNextWave;
        int seconds = ( int )currentTime % 60;
        int minutes = ( int )currentTime / 60;
        _timerText.text = string.Format( "{0:00}:{1:00}", minutes, seconds );
    }

    private void UpdateResourcesText( uint count )
    {
        if ( _resourcesText )
            _resourcesText.text = count.ToString();
    }

    private void UpdateRoundText( uint level )
    {
        if ( _zoneText )
            _zoneText.text = level.ToString();
    }

    private void ShowWarning( GameCell.Quarter quarter )
    {
        int index;
        switch ( quarter )
        {
            case GameCell.Quarter.TopRight:
                index = 0;
                break;
            case GameCell.Quarter.BottomRight:
                index = 1;
                break;
            case GameCell.Quarter.BottomLeft:
                index = 2;
                break;
            case GameCell.Quarter.TopLeft:
                index = 3;
                break;
            default:
                throw new ArgumentOutOfRangeException( "quarter", quarter, null );
        }
        StartCoroutine( ShowWarningCoroutine( index ) );
    }

    private IEnumerator ShowWarningCoroutine( int index )
    {
        GameObject o = _warningImages[index].gameObject;
        for ( int i = 0; i < _warningShowRepetition; ++i )
        {
            o.SetActive( true );
            yield return new WaitForSeconds( _warningShowTime );
            o.SetActive( false );
            yield return new WaitForSeconds( _warningShowTime );
        }
    }
}