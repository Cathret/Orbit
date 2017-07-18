using Orbit.Entity;
using UnityEngine;
using UnityEngine.UI;

public class InsertionMenu : MonoBehaviour
{
    public delegate void DestroyDelegate();

    [SerializeField]
    private GameObject _container;

    [SerializeField]
    private InsertionItem _itemPrefab;

    [SerializeField]
    private Text _nameLabel;
    [SerializeField]
    private AUnitController[] _prefabCells;

    public Text NameLabel
    {
        get { return _nameLabel; }
    }

    public uint X { get; set; }
    public uint Y { get; set; }
    public event DestroyDelegate DestroyCallback;

    // Use this for initialization
    private void Start()
    {
        for ( int i = 0; i < _prefabCells.Length; ++i )
        {
            InsertionItem item = Instantiate( _itemPrefab, _container.transform, false );
            item.SetItem( _prefabCells[i], X, Y );
            item.Menu = this;
            item.DestroyCallback += Quit;
        }
    }

    private void Update()
    {
        if ( Input.GetMouseButtonDown( 0 )
             && _container.gameObject.activeSelf
             && !RectTransformUtility.RectangleContainsScreenPoint(
                                                                   _container.gameObject.GetComponent<RectTransform>(),
                                                                   Input.mousePosition ) )
            Quit();
    }

    private void Quit()
    {
        Destroy( gameObject );
    }

    private void OnDestoy()
    {
        if ( DestroyCallback != null )
            DestroyCallback.Invoke();
    }
}