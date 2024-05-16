using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{
    private static List<ProjectileLine> _lines = new();

    private const float DimFactor = 0.75f;

    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _projectile = GetComponentInParent<Projectile>();

        AddLine(this);
    }

    private void Start()
    {
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);
    }

    private void FixedUpdate()
    {
        if (!_drawing) return;

        _line.SetPosition(_line.positionCount++, transform.position);

        if (!_projectile) return;
        if (!_projectile.Sleeping) return;

        _drawing = false;
        _projectile = null;
    }

    private void OnDestroy()
    {
        _lines.Remove(this);
    }

    private static void AddLine(ProjectileLine line)
    {
        foreach (var l in _lines)
        {
            var color = l._line.startColor;
            color *= DimFactor;
            l._line.startColor = l._line.endColor = color;
        }

        _lines.Add(line);
    }
}
