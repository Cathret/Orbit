using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private GameManager _instance;

    public GameManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    public enum State
    {
        PLAYING,
        PAUSE,
        GAME_OVER
    }

    [SerializeField]
    private bool _canPlay = false;

    [SerializeField]
    private bool _canBuild = false;

    public UnityEvent OnPlay;
    public UnityEvent OnPause;
    public UnityEvent OnGameOver;

    private State _currentState = State.PLAYING;
    public State CurrentState {
        get { return _currentState; }
        private set
        {
            switch ( value )
            {
                case State.PLAYING:
                    if (OnPlay != null)
                        OnPlay.Invoke();
                    break;
                case State.PAUSE:
                    if (OnPause != null)
                        OnPause.Invoke();
                    break;
                case State.GAME_OVER:
                    if (OnGameOver != null)
                        OnGameOver.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException( "value", value, null );
            }
            _currentState = value;
        }
    }

    public float CurrentTime { get; private set; }

    // Use this for initialization
    void Start ()
    {
        CurrentTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if ( CurrentState == State.PLAYING )
	        CurrentTime += Time.deltaTime;
	}
}
