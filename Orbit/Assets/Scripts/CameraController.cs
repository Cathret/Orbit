using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField]
    private GameGrid _grid;

    [SerializeField]
    private float _speed;
    public uint Padding;

    private float _targetOrthographicSize;

    void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _targetOrthographicSize = _mainCamera.orthographicSize;
    }
	// Use this for initialization
	void Start ()
	{
	    Vector3 position = _grid.gameObject.transform.position;
	    float z = _mainCamera.transform.position.z;
	    position.z = z;

	    transform.position = position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!Mathf.Approximately(_mainCamera.orthographicSize, _targetOrthographicSize))
	        Mathf.Lerp(_mainCamera.orthographicSize, _targetOrthographicSize, Time.deltaTime * _speed);
	}

    void UpdateTargetOrthoSize()
    {
        float f = -_mainCamera.transform.position.z;
        Vector3 center = _mainCamera.ViewportToWorldPoint(new Vector3(0.5F, 0.5F, f));
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0.0F, 0.0F, f));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0F, f));

        float left = bottomLeft.x - center.x;
        float right = topRight.x - center.x;
        float top = topRight.y - center.y;
        float bottom = bottomLeft.y - center.y;


        uint gridEfficientPosX = _grid.EfficientPosX;
        uint gridEfficientPosY = _grid.EfficientPosY;
        float gridCellSize = _grid.CellSize;

        Vector2 targetBottomLeft;
        targetBottomLeft.x = (gridEfficientPosX - Padding) * gridCellSize;
        targetBottomLeft.y = (gridEfficientPosY - Padding) * gridCellSize;

        Vector2 targetTopRight;
        targetTopRight.x = (gridEfficientPosX + _grid.EfficientWidth + Padding) * gridCellSize;
        targetTopRight.y = (gridEfficientPosY + _grid.EfficientHeight + Padding) * gridCellSize;

        float targetLeft = targetBottomLeft.x - center.x;
        float targetRight = targetTopRight.x - center.x;
        float targetTop = targetTopRight.y - center.y;
        float targetBottom = targetBottomLeft.y - center.y;

        float max = -1;

        float ratioLeft = targetLeft / left;
        max = ratioLeft > max ? ratioLeft : max;

        float ratioRight = targetRight / right;
        max = ratioRight > max ? ratioRight : max;

        float ratioTop = targetTop / top;
        max = ratioTop > max ? ratioTop : max;

        float ratioBottom = targetBottom / bottom;
        max = ratioBottom > max ? ratioBottom : max;

        _targetOrthographicSize = _mainCamera.orthographicSize * max;
    }
}
