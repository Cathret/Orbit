using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InsertionMenu : MonoBehaviour
{
    [SerializeField]
    private AUnitController[] _prefabCells;

    [SerializeField]
    private InsertionItem _itemPrefab;

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private Text _nameLabel;

	public Text NameLabel
	{
		get { return _nameLabel; }
	}

	public uint X { get; set; }
	public uint Y { get; set; }

    public delegate void DestroyDelegate();
    public event DestroyDelegate DestroyCallback;

    // Use this for initialization
    void Start()
    {
        for ( int i = 0; i < _prefabCells.Length; ++i )
        {
			InsertionItem item = Instantiate( _itemPrefab, _container.transform, false );
            item.SetItem(_prefabCells[i], X, Y);
			item.Menu = this;
            item.DestroyCallback += Quit;
        }
    }

    void Update()
    {
		if (Input.GetMouseButtonDown(0) && _container.gameObject.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
				_container.gameObject.GetComponent<RectTransform>(),
                                                               Input.mousePosition))
        {
            Quit();
        }
    }

    void Quit()
    {
        Destroy( gameObject );
    }

    void OnDestoy()
    {
        if ( DestroyCallback != null )
            DestroyCallback.Invoke();
    }

}