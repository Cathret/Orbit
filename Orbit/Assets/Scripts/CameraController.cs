using Kino;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GridOverlay _caseGridOverlay;
    private float _fixedZ;
    private Camera _mainCamera;

    private GridOverlay _quarterGridOverlay;

    [SerializeField]
    private float _resizeSpeed = 15.0f;
    private float _targetOrthographicSize;

    private Vector3 _targetPosition;

    [SerializeField]
    private float _translationSpeed = 5.0f;

    public uint Padding = 10;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _targetOrthographicSize = _mainCamera.orthographicSize;
        _targetPosition = transform.position;
        _fixedZ = transform.position.z;
        GridOverlay[] gridOverlays = GetComponents<GridOverlay>();
        _quarterGridOverlay = gridOverlays[0];
        _caseGridOverlay = gridOverlays[1];
    }

    // Use this for initialization
    private void Start()
    {
        GameGrid.Instance.OnLayoutChanged.AddListener( UpdateTarget );
        UpdateTarget();
        transform.position = _targetPosition;

        Bloom component = GetComponent<Bloom>();
        if ( component )
            component.enabled = PlayerPrefs.GetInt( "BLOOM_EFFECT", 1 ) == 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if ( GameManager.Instance.CurrentGameState != GameManager.GameState.Play )
            return;
        _mainCamera.orthographicSize = Mathf.Lerp( _mainCamera.orthographicSize, _targetOrthographicSize,
                                                   Time.deltaTime * _resizeSpeed );
        transform.position = Vector3.Lerp( transform.position, _targetPosition, Time.deltaTime * _translationSpeed );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( _targetPosition, 1.0f );
    }

    private void UpdateTarget()
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

        _targetPosition = grid.RealCenter;
        _targetPosition.z = _fixedZ;

        _targetOrthographicSize = _mainCamera.orthographicSize * ratio;

        float side = Mathf.Max( width, height );
        if ( _quarterGridOverlay )
        {
            _quarterGridOverlay.startX = _targetPosition.x - side;
            _quarterGridOverlay.startY = _targetPosition.y - side;

            _quarterGridOverlay.gridSizeX = _targetPosition.x + side;
            _quarterGridOverlay.gridSizeY = _targetPosition.y + side;
            _quarterGridOverlay.step = side;
        }

        if ( _caseGridOverlay )
        {
            int countCell = ( int )( side / cellSize );
            float realSide = ( countCell + 1 ) * cellSize;
            _caseGridOverlay.startX = _targetPosition.x - realSide;
            _caseGridOverlay.startY = _targetPosition.y - realSide;

            _caseGridOverlay.gridSizeX = _targetPosition.x + realSide;
            _caseGridOverlay.gridSizeY = _targetPosition.y + realSide;
            _caseGridOverlay.step = cellSize;
        }
    }
}