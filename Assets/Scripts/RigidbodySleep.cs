using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodySleep : MonoBehaviour
{
    private Rigidbody _rb;
    private int _countdown = 4;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_countdown-- > 0) _rb.Sleep();
    }
}
