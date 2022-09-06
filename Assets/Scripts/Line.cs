using UnityEngine;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private EdgeCollider2D _collider;
    int colorHash = Shader.PropertyToID("_Color");

    private readonly List<Vector2> _points = new();

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponentInChildren<EdgeCollider2D>();
    }

    private void Start()
    {
        _collider.transform.position -= transform.position;
    }


    public void SetPosition(Vector2 position)
    {
        if (!CanAppend(position))
            return;

        _points.Add(position);

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);

        _collider.points = _points.ToArray();
    }

    private bool CanAppend(Vector2 position)
    {
        if (_lineRenderer.positionCount == 0)
            return true;

        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), position) > DrawManager.Resolution;
    }

    public void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
}
