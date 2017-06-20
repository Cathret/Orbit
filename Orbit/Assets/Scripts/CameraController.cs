using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField]
    private GameGrid _grid;

    [SerializeField]
    private float _speed = 20.0f;
    public uint Padding = 2;

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
	void Start ()
	{
	    if (_grid)
	        _grid.OnLayoutChanged.AddListener(UpdateTarget);
	}
	
	// Update is called once per frame
	void Update ()
	{
        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _targetOrthographicSize, Time.deltaTime * _speed);
	    transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);
	}

    void UpdateTarget()
    {
        if (!_grid)
            return;

        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0.0F, 0.0F, -_fixedZ));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0F, -_fixedZ));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        float cellSize = _grid.CellSize;

        float largeSide = (_grid.EfficientSide + Padding) * cellSize;

        float ratioWidth = largeSide / width;
        float ratioHeight = largeSide / height;

        float ratio = ratioWidth > ratioHeight ? ratioWidth : ratioHeight;

        _targetPosition = new Vector3(_grid.CenterX * cellSize, _grid.CenterY * cellSize, _fixedZ);
        _targetOrthographicSize = _mainCamera.orthographicSize * ratio;
    }
}
