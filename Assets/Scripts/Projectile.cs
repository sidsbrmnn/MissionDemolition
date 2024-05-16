using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private static readonly List<Projectile> Projectiles = new();

    private const int LookBackCount = 10;

    public bool Sleeping { get; private set; }

    private Rigidbody _rb;
    private Vector3 _previousPosition;
    private List<float> _deltas;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        Sleeping = false;
        _previousPosition = new Vector3(1000, 1000, 0);
        _deltas = new List<float> { 1000 };

        Projectiles.Add(this);
    }

    private void FixedUpdate()
    {
        if (_rb.isKinematic || Sleeping) return;

        var delta = Vector3.Distance(transform.position, _previousPosition);
        _deltas.Add(delta);

        _previousPosition = transform.position;

        while (_deltas.Count > LookBackCount) _deltas.RemoveAt(0);

        var max = _deltas.Prepend(0f).Max();
        if (max > Physics.sleepThreshold) return;

        Sleeping = true;
        _rb.Sleep();
    }

    private void OnDestroy()
    {
        Projectiles.Remove(this);
    }

    public static void Clear()
    {
        foreach (var p in Projectiles) Destroy(p.gameObject);
    }
}
