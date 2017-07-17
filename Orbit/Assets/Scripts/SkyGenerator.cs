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
    private float _minGrayScale = 0.3f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float _maxGrayScale = 0.7f;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _grayScaleVariation = 0.1f;

    [SerializeField]
    private float _minLoopLength = 0.5f;

    [SerializeField]
    private float _maxLoopLength = 1.0f;

    [SerializeField]
    private float _loopVariation = 0.2f;

    public float Density = 0.05f;

    public float CurrentDensity
    {
        get
        {
            float fixedZ = -Camera.main.transform.position.z;
            Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0.0F, 0.0F, fixedZ));
            Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0F, fixedZ));
            return _spriteObjects.Count / ( ( topRight.x - bottomLeft.x ) * ( topRight.y - bottomLeft.y ) );
        }
    }
    [SerializeField]
    private StarDecoration[] _spritePrefabs;

    private readonly List<StarDecoration> _spriteObjects = new List<StarDecoration>();

    void GenStar()
    {
        if ( Camera.main == null )
            return;
        int index = Random.Range(0, _spritePrefabs.Length);
        StarDecoration star = Instantiate(_spritePrefabs[index], transform);

        star.GrayScale = Random.Range(_minGrayScale, _maxGrayScale);
        star.GrayScaleVariation = Random.Range(0.0f, _grayScaleVariation);

        star.ScaleVariation = Random.Range(0.0f, _scaleVariation);
        star.LoopLength = Random.Range(_minLoopLength, _maxLoopLength) + Random.Range(-_loopVariation, _loopVariation);

        Vector2 pos = new Vector2(Random.Range( 0.0f, 1.0f ), Random.Range(0.0f, 1.0f));

        star.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(pos.x, pos.y
                                                                               , -Camera.main.transform.position.z));
        star.OnInvisble += ReleaseStar;
        _spriteObjects.Add( star );
    }

    void ReleaseStar(StarDecoration star)
    {
        int indexOf = _spriteObjects.IndexOf( star );
        Destroy( star.gameObject );
        _spriteObjects.RemoveAt( indexOf );
    }

    public void Clear()
    {
        for ( int i = 0; i < _spriteObjects.Count; ++i )
            Destroy(_spriteObjects[i].gameObject);

        _spriteObjects.Clear();
    }

    void Update()
    {
        while (CurrentDensity < Density)
        {
            GenStar();
        }
    }
}
