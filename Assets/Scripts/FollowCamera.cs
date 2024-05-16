using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    public enum View
    {
        None,
        Slingshot,
        Castle,
        Both
    }

    public static FollowCamera Instance { get; private set; }

    public GameObject Target { get; set; }

    [SerializeField] private float easing = 0.05f;
    [SerializeField] private Vector2 minPosition = Vector2.zero;
    [SerializeField] private GameObject viewBoth;

    private Camera _mainCamera;
    private float _zOffset;
    private View _nextView = View.Slingshot;

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _zOffset = Position.z;
    }

    private void FixedUpdate()
    {
        var destination = Vector3.zero;

        if (Target)
        {
            var rb = Target.GetComponent<Rigidbody>();
            if (rb && rb.IsSleeping()) Target = null;
        }

        if (Target) destination = Target.transform.position;

        destination.x = Mathf.Max(minPosition.x, destination.x);
        destination.y = Mathf.Max(minPosition.y, destination.y);
        destination = Vector3.Lerp(Position, destination, easing);
        destination.z = _zOffset;

        Position = destination;
        _mainCamera.orthographicSize = destination.y + 10;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SwitchView(View view)
    {
        if (view == View.None) view = _nextView;

        switch (view)
        {
            case View.Slingshot:
                Target = null;
                _nextView = View.Castle;
                break;
            case View.Castle:
                Target = GameManager.Instance.Castle;
                _nextView = View.Both;
                break;
            case View.Both:
                Target = viewBoth;
                _nextView = View.Slingshot;
                break;
        }
    }

    public void SwitchView() => SwitchView(View.None);

    private Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
