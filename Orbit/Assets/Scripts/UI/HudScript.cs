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
    void Start ()
	{
	    GameManager.Instance.OnResourcesChange += UpdateResourcesText;
	    WaveManager.Instance.RoundChanged += UpdateRoundText;
        _rotateClockWise.onClick.AddListener( GameGrid.Instance.RotateClockwise );
        _rotateIInvClockWise.onClick.AddListener(GameGrid.Instance.RotateReverseClockwise);

    }
	
	// Update is called once per frame
	void Update ()
	{
	    float currentTime = GameManager.Instance.CurrentTime;
	    int seconds = (( int )currentTime) % 60;
        int minutes = ((int)currentTime) / 60;
        _timerText.text = String.Format("{0:00}:{1:00}", minutes, seconds);

    }

    void UpdateResourcesText( uint count )
    {
        _resourcesText.text = count.ToString();
    }

    void UpdateRoundText( uint level )
    {
        _zoneText.text = level.ToString();
    }
}
