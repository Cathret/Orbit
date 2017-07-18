using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class StarDecoration : MonoBehaviour
{
    public delegate void SelfDelegate( StarDecoration obj );

    private Vector3 _defaultScale;

    private SpriteRenderer _spriteRenderer;
    private float _timer;

    [Range( 0.0f, 1.0f )]
    public float GrayScale = 0.5f;
    [Range( 0.0f, 1.0f )]
    public float GrayScaleVariation = 0.1f;

    public float LoopLength = 0.5f;

    public float ScaleVariation;

    public event SelfDelegate OnInvisble;

    private void Awake()
    {
        _defaultScale = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _timer = Random.Range( 0.0f, LoopLength );
    }


    // Update is called once per frame
    private void Update()
    {
        UpdateDesign( Time.deltaTime );
    }

    private void UpdateDesign( float deltaTime )
    {
        _timer += deltaTime;
        if ( Mathf.Approximately( _timer, LoopLength * 2 ) )
            _timer = 0.0f;

        float variation = Mathf.Sin( Mathf.PI / 2 * ( _timer / LoopLength ) );

        float f = GrayScale + GrayScaleVariation * variation;
        _spriteRenderer.color = new Color( f, f, f );

        transform.localScale = _defaultScale + _defaultScale * ( ScaleVariation * variation );
    }

    private void OnBecameInvisible()
    {
        if ( OnInvisble != null )
            OnInvisble( this );
    }

    public void CheckIfVisible()
    {
        if ( _spriteRenderer.isVisible )
            if ( OnInvisble != null )
                OnInvisble( this );
    }
}