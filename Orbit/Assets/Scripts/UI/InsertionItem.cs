using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InsertionItem : MonoBehaviour
{
    public delegate void DestroyDelegate();

    private InsertionMenu _menu;
    [SerializeField]
    private Button button;

    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    public InsertionMenu Menu
    {
        set { _menu = value; }
    }

    public uint X { get; private set; }
    public uint Y { get; private set; }

    public AUnitController Unit { get; private set; }

    public event DestroyDelegate DestroyCallback;


    public void SetItem( AUnitController unit, uint x, uint y )
    {
        if ( unit == null )
            return;
        X = x;
        Y = y;
        Unit = unit;
        image.sprite = Unit.Icon;
        text.text = Unit.Price.ToString();
        if ( Unit.Price <= GameManager.Instance.ResourcesCount )
        {
            button.onClick.AddListener( AddUnit );
        }
        else
        {
            button.enabled = false;
            text.color = Color.red;
            image.color = Color.grey;
        }
    }

    private void AddUnit()
    {
        GameManager.Instance.ResourcesCount -= Unit.Price;
        GameGrid.Instance.AddCase( Unit.Cell, X, Y );
        if ( DestroyCallback != null )
            DestroyCallback.Invoke();
    }

    public void ShowName()
    {
        if ( _menu )
            _menu.NameLabel.text = Unit.UnitName;
    }
}