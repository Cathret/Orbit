using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    enum ClickType
    {
        SELECT_CELL,
        SELECT_UNIT,
        NONE
    }

    public GameGrid _grid;
    private ClickType type = ClickType.NONE;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if ( _grid && type == ClickType.SELECT_CELL )
	    {
	        Highlight(Input.mousePosition);

	    }
	}

    public void Highlight(Vector2 mousePos)
    {
        if (!_grid)
            return;

        float cellSize = _grid.CellSize;
        float side = _grid.Side;

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _grid.FixedZ));

        int posX = (int)(pos.x / cellSize);
        int posY = (int)(pos.y / cellSize);

        if (posX > 0 && posX < side)
        {
            if (posY > 0 && posY < side)
            {
                //_highlightedPos = new Vector2(posX, posY);
            }
        }
    }

}
