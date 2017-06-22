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

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.OnResourcesChange += UpdateResourcesText;
        UpdateResourcesText( GameManager.Instance.ResourcesCount );
        WaveManager.Instance.RoundChanged += UpdateRoundText;
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
        float currentTime = GameManager.Instance.CurrentTime;
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
}