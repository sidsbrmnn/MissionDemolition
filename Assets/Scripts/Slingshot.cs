using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class Slingshot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float velocity = 10f;
    [SerializeField] private GameObject projectileLinePrefab;

    private Camera _mainCamera;
    private SphereCollider _collider;
    private GameObject _launchPoint;
    private GameObject _projectile;
    private bool _isAiming;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _collider = GetComponent<SphereCollider>();

        _launchPoint = transform.Find("Launch Point").gameObject;
        _launchPoint.SetActive(false);
    }

    private void Update()
    {
        if (!_isAiming) return;

        var mousePosition = Input.mousePosition;
        mousePosition.z = -_mainCamera.transform.position.z;
        var worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        var direction = worldPosition - _launchPoint.transform.position;
        if (direction.magnitude > _collider.radius)
            direction = direction.normalized * _collider.radius;

        _projectile.transform.position = _launchPoint.transform.position + direction;

        if (Input.GetMouseButtonUp(0))
        {
            _isAiming = false;

            var rb = _projectile.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.velocity = -direction * velocity;

            FollowCamera.Instance.SwitchView(FollowCamera.View.Slingshot);

            FollowCamera.Instance.Target = _projectile;
            Instantiate(projectileLinePrefab, _projectile.transform);
            _projectile = null;

            GameManager.Instance.ShotFired();
        }
    }

    private void OnMouseEnter()
    {
        _launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        _launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        _isAiming = true;
        _projectile = Instantiate(projectilePrefab, _launchPoint.transform.position, Quaternion.identity);
        _projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
