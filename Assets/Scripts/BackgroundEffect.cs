using UnityEngine;

public class BackgroundEffect : MonoBehaviour
{
    private Material _mat;
    private Vector2 _offset;
    private float _step;
    private float _speed;

    private void Awake()
    {
        _mat = GetComponent<MeshRenderer>().material;
        _step = 0f;
        _speed = -0.05f;
        _offset = Vector2.one;
    }

    private void Update()
    {
        _offset.x = _step;
        _mat.mainTextureOffset = _offset;
        _step += Time.deltaTime * _speed;
        //_step %= 1f;
    }
}
