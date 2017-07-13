using System.Collections;
using System.Collections.Generic;
using Kino;
using Orbit.Entity;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField]
    private float _resizeSpeed = 15.0f;

    [SerializeField]
    private float _translationSpeed = 5.0f;

    public uint Padding = 10;

    private Vector3 _targetPosition;
    private float _targetOrthographicSize;
    private float _fixedZ;

    void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _targetOrthographicSize = _mainCamera.orthographicSize;
        _targetPosition = transform.position;
        _fixedZ = transform.position.z;
    }

    // Use this for initialization
    void Start()
    {
        GameGrid.Instance.OnLayoutChanged.AddListener( UpdateTarget );
        UpdateTarget();
        transform.position = _targetPosition;

        SkyGenerator.Instance.Populate();

        Bloom component = GetComponent<Bloom>();
        if ( component )
        {
            component.enabled = PlayerPrefs.GetInt("BLOOM_EFFECT", 1) == 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (GameManager.Instance.CurrentGameState != GameManager.GameState.Play)
			return;
        _mainCamera.orthographicSize = Mathf.Lerp( _mainCamera.orthographicSize, _targetOrthographicSize,
                                                   Time.deltaTime * _resizeSpeed );
        transform.position = Vector3.Lerp( transform.position, _targetPosition, Time.deltaTime * _translationSpeed );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( _targetPosition, 1.0f );
    }

    void UpdateTarget()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint( new Vector3( 0.0F, 0.0F, -_fixedZ ) );
        Vector3 topRight = _mainCamera.ViewportToWorldPoint( new Vector3( 1.0f, 1.0F, -_fixedZ ) );

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        GameGrid grid = GameGrid.Instance;

        float cellSize = grid.CellSize;

        float largeSide = ( grid.EfficientSide + Padding ) * cellSize;

        float ratioWidth = largeSide / width;
        float ratioHeight = largeSide / height;

        float ratio = ratioWidth > ratioHeight ? ratioWidth : ratioHeight;

        _targetPosition = GameGrid.Instance.RealCenter;
        _targetPosition.z = _fixedZ;

        _targetOrthographicSize = _mainCamera.orthographicSize * ratio;
    }
}