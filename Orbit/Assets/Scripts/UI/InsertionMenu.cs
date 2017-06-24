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

    public uint X;
    public uint Y;
    private bool _initialized = false;

    public delegate void DestroyDelegate();
    public event DestroyDelegate DestroyCallback;

    // Use this for initialization
    void Start()
    {
        for ( int i = 0; i < _prefabCells.Length; ++i )
        {
            InsertionItem item = Instantiate( _itemPrefab, transform, false );
            item.SetItem(_prefabCells[i], X, Y);
            item.DestroyCallback += Quit;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                                                               gameObject.GetComponent<RectTransform>(),
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