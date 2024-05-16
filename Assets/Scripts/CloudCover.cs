using UnityEngine;

[DisallowMultipleComponent]
public class CloudCover : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int count = 40;
    [SerializeField] private Vector3 minPosition = new(-20, -5, -5);
    [SerializeField] private Vector3 maxPosition = new(300, 40, 5);

    [Tooltip("x = min, y = max")] [SerializeField]
    private Vector2 scaleRange = new(1, 4);

    private void Start()
    {
        for (var i = 0; i < count; i++)
        {
            var go = new GameObject();

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprites[Random.Range(0, sprites.Length)];

            go.transform.position = RandomPosition();
            go.transform.localScale = Vector3.one * Random.Range(scaleRange.x, scaleRange.y);
            go.transform.SetParent(transform);
        }
    }

    private Vector3 RandomPosition() => new()
    {
        x = Random.Range(minPosition.x, maxPosition.x),
        y = Random.Range(minPosition.y, maxPosition.y),
        z = Random.Range(minPosition.z, maxPosition.z)
    };
}
