using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 15f;

    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        rb.linearVelocity = transform.right * speed;
    }

    void Update()
    {
        DestroyIfOutOfView();
    }

    void DestroyIfOutOfView()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPos.x < 0 || viewportPos.x > 1 ||
            viewportPos.y < 0 || viewportPos.y > 1)
        {
            Destroy(gameObject);
        }
    }
}
