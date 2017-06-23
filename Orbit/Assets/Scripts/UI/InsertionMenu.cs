using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InsertionMenu : MonoBehaviour
{
    [SerializeField]
    private AUnitController[] _prefabCells;

    [SerializeField]
    private InsertionItem _itemPrefab;

    [SerializeField]
    private Button _quitButton;

    public uint X;
    public uint Y;

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

        _quitButton.onClick.AddListener( Quit );
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