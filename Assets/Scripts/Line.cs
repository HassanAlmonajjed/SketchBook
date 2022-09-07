using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private EdgeCollider2D _collider;

    private List<Vector2> _points = new();
    public int PointCount => _lineRenderer.positionCount;

    private void Start()
    {
        _collider.transform.position -= transform.position;
    }

    public void SetPosition(Vector2 position, bool ForceAppend = false)
    {
        if (!CanAppend(position) && !ForceAppend)
            return;

        _points.Add(position);

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);

        _collider.points = _points.ToArray();
    }

    public void SetPositions(Vector2[] points)
    {
        _points = points.ToList();
        _lineRenderer.positionCount = _points.Count;

        Vector3[] positions = new Vector3[points.Length];
        for (int i = 0; i < positions.Length; i++)
            positions[i] = points[i];
        _lineRenderer.SetPositions(positions);
        _collider.points = points;
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

    public LineData ExtractData()
    {
        Vector3[] points = new Vector3[_points.Count];
        _lineRenderer.GetPositions(points);
        LineData data = new()
        {
            Position = transform.position,
            Color = _lineRenderer.startColor,
            Points = points
        };

        return data;
    }
}
