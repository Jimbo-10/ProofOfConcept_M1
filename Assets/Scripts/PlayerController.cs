using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Camera mainCamera;

    [SerializeField]
    InputActionAsset inputActions;
    InputAction moveAction;
    InputAction mousePos;
    InputAction shootAction;

    public float moveSpeed = 5f;    
    public float fireRate = 0.2f;

    public float nextFireTime;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        moveAction = inputActions.FindAction("Move");
        mousePos = inputActions.FindAction("Point");
        shootAction = inputActions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        RotatePlayer();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void RotatePlayer()
    {
        Vector2 mouseScreenPos = mousePos.ReadValue<Vector2>();

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;
        
        Vector2 direction = mouseWorldPos - transform.position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //angle -= 90;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void HandleShooting()
    {
        if (shootAction.IsPressed() && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
