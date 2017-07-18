using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _zoneText;
    [SerializeField]
    private Text _resourcesText;
    [SerializeField]
    private Button _rotateClockWise;
    [SerializeField]
    private Button _rotateIInvClockWise;

    [SerializeField, Header("Warning")]
    private Image[] _warningImages;
    [SerializeField]
    private float _warningShowTime = 0.3f;
    [SerializeField]
    private uint _warningShowRepetition = 3;

    void Awake()
    {
        GameManager.Instance.OnResourcesChange += UpdateResourcesText;
        UpdateResourcesText(GameManager.Instance.ResourcesCount);
        WaveManager.Instance.RoundChanged += UpdateRoundText;

        WaveManager.Instance.OnNewWave += ShowWarning;
        for (int i = 0; i < _warningImages.Length; ++i)
            _warningImages[i].gameObject.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        if ( _rotateClockWise )
            _rotateClockWise.onClick.AddListener( GameGrid.Instance.RotateClockwise );

        if ( _rotateIInvClockWise )
            _rotateIInvClockWise.onClick.AddListener( GameGrid.Instance.RotateReverseClockwise );     
    }

    // Update is called once per frame
    void Update()
    {
        if ( !_timerText )
            return;
        float currentTime = WaveManager.Instance.TimeToNextWave;
        int seconds = ( (int)currentTime ) % 60;
        int minutes = ( (int)currentTime ) / 60;
        _timerText.text = String.Format( "{0:00}:{1:00}", minutes, seconds );
    }

    void UpdateResourcesText( uint count )
    {
        if ( _resourcesText )
            _resourcesText.text = count.ToString();
    }

    void UpdateRoundText( uint level )
    {
        if ( _zoneText )
            _zoneText.text = level.ToString();
    }

    void ShowWarning( GameCell.Quarter quarter )
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

    IEnumerator ShowWarningCoroutine( int index )
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