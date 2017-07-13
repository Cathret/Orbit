using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent( typeof( SpriteRenderer ) )]
public class StarDecoration : MonoBehaviour
{

    [Range( 0.0f, 1.0f )]
    public float GrayScale = 0.5f;
    [Range(0.0f, 1.0f)]
    public float GrayScaleVariation = 0.1f;

    public float ScaleVariation = 0.0f;

    private Vector3 _defaultScale;

    public float LoopLength = 0.5f;
    private float _timer = 0.0f;

    private SpriteRenderer _spriteRenderer;

    public delegate void SelfDelegate( StarDecoration obj );

    public event SelfDelegate OnInvisble;

    void Awake()
    {
        _defaultScale = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();     
    }

    void Start()
    {
        _timer = Random.Range(0.0f, LoopLength);
    }


    // Update is called once per frame
    void Update()
    {
        UpdateDesign( Time.deltaTime );
    }

    void UpdateDesign(float deltaTime)
    {
        _timer += deltaTime;
        if ( Mathf.Approximately( _timer, LoopLength * 2 ) )
            _timer = 0.0f;

        float variation = Mathf.Sin( ( Mathf.PI / 2 ) * ( _timer / LoopLength ) );

        float f = GrayScale + GrayScaleVariation * variation;
        _spriteRenderer.color = new Color(f, f, f);

        transform.localScale = _defaultScale + _defaultScale * ( ScaleVariation * variation );
    }

    void OnBecameInvisible()
    {
        if ( OnInvisble != null )
            OnInvisble( this );
    }

    public void CheckIfVisible()
    {
       if ( _spriteRenderer.isVisible )
           if (OnInvisble != null)
               OnInvisble(this);
    }
}