using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    public static bool GoalMet;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Projectile>(out _)) return;

        GoalMet = true;
        var material = GetComponent<Renderer>().material;
        var color = material.color;
        color.a = 0.75f;
        material.color = color;
    }
}
