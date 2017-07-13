using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyGenerator : MonoBehaviour
{

    private static SkyGenerator _instance;

    public static SkyGenerator Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SkyGenerator>();
            return _instance;
        }
    }

    [SerializeField, Range(0.0f, 1.0f)]
    private float _scaleVariation = 0.1f;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _grayScale = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float _grayScaleVariation = 0.1f;

    [SerializeField]
    private float LoopLength = 0.7f;

    [SerializeField]
    private float LoopVariation = 0.2f;

    [SerializeField]
    private uint StarCount = 50;

    [SerializeField]
    private StarDecoration[] _spritePrefabs;

    private List<StarDecoration> _spriteObjects = new List<StarDecoration>();

    void GenStar()
    {
        int index = Random.Range(0, _spritePrefabs.Length);
        StarDecoration star = Instantiate(_spritePrefabs[index], transform);

        star.GrayScale = _grayScale;
        star.GrayScaleVariation = Random.Range(0.0f, _grayScaleVariation);

        star.ScaleVariation = Random.Range(0.0f, _scaleVariation);
        star.LoopLength = LoopLength + Random.Range(-LoopVariation, LoopVariation);

        Vector2 insideUnitCircle = Random.insideUnitCircle;
        insideUnitCircle.x = Mathf.Abs(insideUnitCircle.x);
        insideUnitCircle.y = Mathf.Abs(insideUnitCircle.y);

        star.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(insideUnitCircle.x, insideUnitCircle.y
                                                                               , -Camera.main.transform.position.z));
        star.OnInvisble += ReleaseStar;
        _spriteObjects.Add( star );
    }

    void ReleaseStar(StarDecoration star)
    {
        int indexOf = _spriteObjects.IndexOf( star );
        Destroy( star.gameObject );
        _spriteObjects.RemoveAt( indexOf );
        GenStar();
    }

    public void Clear()
    {
        for ( int i = 0; i < _spriteObjects.Count; ++i )
            Destroy(_spriteObjects[i].gameObject);

        _spriteObjects.Clear();
    }

    public void Populate()
    {
        Clear();
        for (int i = 0; i < StarCount; ++i)
        {
            GenStar();
        }
    }

    public void CheckAllStars()
    {
        for(int i = 0; i < StarCount; ++i )
        {
            _spriteObjects[i].CheckIfVisible();
        }
    }
}
