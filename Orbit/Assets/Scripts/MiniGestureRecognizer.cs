using System;
using UnityEngine;

public class MiniGestureRecognizer : MonoBehaviour
{
    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private enum GestureType
    {
        DragnDrop,
        Else
    }

    public static event Action<SwipeDirection> Swipe;
    public static event Action<Vector2> Click;

    public static event Action<Vector2> OnDragStart;
    public static event Action<Vector2> OnDrag;
    public static event Action<Vector2> OnDrop;

    private GestureType _gestureType = GestureType.Else;

    [SerializeField]
    private float _minSwipeDist = 50.0f;
    [SerializeField]
    private float _maxSwipeTime = 0.5f;
    [SerializeField]
    private float _maxClickDist = 10.0f;
    [SerializeField]
    private float _clickTime = 0.3f;

    private float _timer = 0.0f;

    private Vector2 _firstPosition;
    private Vector2 _lastPosition;

    void Start()
    {
        _timer = float.MaxValue;
    }

    void Update()
    {
        _lastPosition = Input.mousePosition;

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            _timer = 0.0f;
            _firstPosition = _lastPosition;
        }
        else if ( Input.GetMouseButton( 0 ) )
        {
            _timer += Time.deltaTime;

            if ( _timer > _clickTime && _timer > _maxSwipeTime )
            {
                if ( _gestureType == GestureType.DragnDrop )
                {
                    if ( OnDrag != null )
                        OnDrag( _lastPosition );
                }
                else
                {
                    if (OnDragStart != null)
                        OnDragStart(_firstPosition);
                    _gestureType = GestureType.DragnDrop;
                }
            }
        }
        else if ( Input.GetMouseButtonUp( 0 ) )
        {
            if ( _gestureType == GestureType.DragnDrop )
            {
                if ( OnDrop != null )
                    OnDrop( _lastPosition );
            }
            else
            {
                Vector2 direction = _lastPosition - _firstPosition;
                float gestureDist = direction.magnitude;

                if ( _timer < _maxSwipeTime && gestureDist > _minSwipeDist )
                {
                    if ( Swipe != null )
                    {
                        if ( Mathf.Abs( direction.x ) > Mathf.Abs( direction.y ) )
                            Swipe( direction.x > 0 ? SwipeDirection.Right : SwipeDirection.Left );
                        else
                            Swipe( direction.y > 0 ? SwipeDirection.Up : SwipeDirection.Down );
                    }

                }
                else if ( _timer < _clickTime && gestureDist < _maxClickDist )
                {
                    if ( Click != null )
                        Click( _lastPosition );
                }
                else
                {
                    if (OnDragStart != null)
                        OnDragStart(_firstPosition);
                    if (OnDrag != null)
                        OnDrag(_lastPosition);
                    if (OnDrop != null)
                        OnDrop(_lastPosition);
                }
            }

            _gestureType = GestureType.Else;
            _timer = float.MaxValue;
        }
    }
}
