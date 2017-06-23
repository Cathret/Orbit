using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class ManagementMenu : MonoBehaviour
{
    [SerializeField]
    private Button moveButton;
    [SerializeField]
    private Button removeButton;

    public AUnitController unit;

    private bool isDragging = false;

    private uint dragX = 0;
    private uint dragY = 0;

    public delegate void DestroyDelegate();
    public event DestroyDelegate DestroyCallback;

    // Use this for initialization
    void Start ()
	{
	    moveButton.onClick.AddListener( Drag );
	    removeButton.onClick.AddListener(Remove);
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 pos =
                Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
            int x, y;
            if ( GameGrid.Instance.GetPositionFromWorldPoint( pos, out x, out y ) )
            {
                uint ux = ( uint )x;
                uint uy = ( uint )y;
                if ( GameGrid.Instance.CanBeAdded( ux, uy ) )
                {
                    dragX = ux;
                    dragY = uy;
                    unit.Cell.SetPosition(dragX, dragY);
                }
            }
            if ( Input.GetMouseButtonDown( 0 ))
                Drop();
        }
    }

    void Remove()
    {
        GameManager.Instance.ResourcesCount += unit.CurrentPrice;
        GameGrid.Instance.RemoveCell(unit.Cell);
        Quit();
    }

    void Drag()
    {
        isDragging = true;
        GameGrid.Instance.CleanCase(unit.Cell.X, unit.Cell.Y);
        Destroy(moveButton.gameObject);
        Destroy(removeButton.gameObject);
    }

    void Drop()
    {
        isDragging = false;
        GameGrid.Instance.MoveCell(unit.Cell, dragX, dragY);
        Quit();
    }

    void Quit()
    {
        Destroy( gameObject );
    }

    void OnDestoy()
    {
        if (DestroyCallback != null)
            DestroyCallback.Invoke();
    }
}
