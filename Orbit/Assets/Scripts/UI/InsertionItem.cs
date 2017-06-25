using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InsertionItem : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;

    public uint X;
    public uint Y;

    private AUnitController _unit;

    public delegate void DestroyDelegate();

    public event DestroyDelegate DestroyCallback;


    public void SetItem( AUnitController unit, uint x, uint y )
    {
        if (unit == null)
            return;
        X = x;
        Y = y;
        _unit = unit;
        image.sprite = _unit.Icon;
        text.text = _unit.Price.ToString();
		if (_unit.Price <= GameManager.Instance.ResourcesCount)
			button.onClick.AddListener (AddUnit);
		else 
		{
			text.color = Color.red;
			image.color = Color.grey;
		}
    }

    void AddUnit()
    {
		GameManager.Instance.ResourcesCount -= _unit.Price;
		GameGrid.Instance.AddCase (_unit.Cell, X, Y);
		if (DestroyCallback != null)
			DestroyCallback.Invoke ();
    }
}
