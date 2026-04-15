using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    Tilemap mapTilemap;

    private Camera mainCamera;

    [SerializeField]
    InputActionAsset inputActions;
    InputAction moveAction;
    InputAction mousePos;
    InputAction shootAction;

    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float fireRate = 0.2f;
    [SerializeField]
    float nextFireTime;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public AudioSource audioSource;
    public AudioClip shootSound;

    UIManager uiManager;
    MainMenuUI ui;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiManager = FindObjectOfType<UIManager>();
        ui = FindObjectOfType<MainMenuUI>();
        mainCamera = Camera.main;

        moveAction = inputActions.FindAction("Move");
        mousePos = inputActions.FindAction("Point");
        shootAction = inputActions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude < 0.1f)
        {
            moveInput = Vector2.zero;
        }
        RotatePlayer();
        Shoot();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
       // KeepPlayerInsideMap();
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

    void Shoot()
    {
        if (shootAction.IsPressed() && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(shootSound);
        }
       
    }

    /*void KeepPlayerInsideMap()
    {
        Bounds bounds = mapTilemap.localBounds;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);

        transform.position = pos;
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            uiManager.TakeDamage(2);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Zombie"))
        {
            uiManager.TakeDamage(10);
            Destroy(collision.gameObject);
        }

        if (uiManager.currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
