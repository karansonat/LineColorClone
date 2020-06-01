using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer _line;
    private Vector3 _lastPos;
    
    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, transform.position);
        _lastPos = transform.position;
    }

    public void LateUpdate()
    {
        if (_lastPos == transform.position)
            return;

        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, transform.position);
        _lastPos = transform.position;
    }

    public void SetColor(Color color)
    {
        _line.material.color = color;
    }
}
