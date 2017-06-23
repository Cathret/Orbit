using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class ManagementMenu : MonoBehaviour
{
    
    private Button moveButton;

    private Button removeButton;

    public AUnitController unit;

    private bool isDragging = false;

    private uint dragX = 0;
    private uint dragY = 0;
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
                }
            }
            if ( Input.GetMouseButtonUp( 0 ))
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
    }

    void Drop()
    {
        isDragging = false;
        unit.Cell.SetPosition( dragX, dragY );
        Quit();
    }
    void Quit()
    {
        Destroy( gameObject );
    }
}
