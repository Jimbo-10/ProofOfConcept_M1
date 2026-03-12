using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField]
    float speed = 15f;

    Rigidbody2D rb;
    Camera mainCamera;

    UIManager uiManager;
    EnemyStats stats;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        rb.linearVelocity = transform.right * speed;

        uiManager = FindObjectOfType<UIManager>();
        stats = FindObjectOfType<EnemyStats>();
    }

    void Update()
    {
        DestroyBulletOutOfView();
    }

    void DestroyBulletOutOfView()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);

        if (viewPos.x < 0 || viewPos.x > 1 ||
            viewPos.y < 0 || viewPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {          
            if(stats != null)
            {
                stats.TakeDamage(50);
            }

            if (stats.currentHealth <= 0 && uiManager != null)
            {
                uiManager.AddKill();
                Destroy(collision.gameObject);
            }

            Destroy(gameObject);
        }

        else if (collision.CompareTag("Zombie"))
        {
            if (stats != null)
            {
                stats.TakeDamage(15);
            }

            if (stats.currentHealth <= 0 && uiManager != null)
            {
                uiManager.AddKill();
                Destroy(collision.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
