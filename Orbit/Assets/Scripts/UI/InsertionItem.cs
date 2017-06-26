using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InsertionItem : MonoBehaviour
{
    [SerializeField]
    private Button button;

	private InsertionMenu _menu;
	public InsertionMenu Menu 
	{
		set { _menu = value; }
	}

    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;

	public uint X { get; private set; }
	public uint Y { get; private set; }

	public AUnitController Unit { get; private set; }

    public delegate void DestroyDelegate();

    public event DestroyDelegate DestroyCallback;


    public void SetItem( AUnitController unit, uint x, uint y )
    {
        if (unit == null)
            return;
        X = x;
        Y = y;
		Unit = unit;
		image.sprite = Unit.Icon;
		text.text = Unit.Price.ToString();
		if (Unit.Price <= GameManager.Instance.ResourcesCount)
			button.onClick.AddListener (AddUnit);
		else 
		{
			button.enabled = false;
			text.color = Color.red;
			image.color = Color.grey;
		}
    }

    void AddUnit()
    {
		GameManager.Instance.ResourcesCount -= Unit.Price;
		GameGrid.Instance.AddCase (Unit.Cell, X, Y);
		if (DestroyCallback != null)
			DestroyCallback.Invoke ();
    }

	public void ShowName ()
	{
		if (_menu)
			_menu.NameLabel.text = Unit.UnitName;
	}
}
