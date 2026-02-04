using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnEnemy : MonoBehaviour
{
    public Transform player;
    public Tilemap floorTilemap;
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float minSpawnDistance = 4f;
    public float maxSpawnDistance = 8f;
    public float spawnInterval = 2f;
    public int maxEnemies = 20;

    private BoundsInt bounds;
    private float timer;

    void Start()
    {
        bounds = floorTilemap.cellBounds;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval &&
            GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            EnemySpawn();
            timer = 0f;
        }
    }

    void EnemySpawn()
    {
        Vector3 spawnPos = GetValidSpawnPosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int i = 0; i < 50; i++) // safety loop
        {
            // 1️⃣ Random position around player
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector2 candidatePos = (Vector2)player.position + direction * distance;

            // 2️⃣ Convert world → cell
            Vector3Int cellPos = floorTilemap.WorldToCell(candidatePos);

            // 3️⃣ Check inside tilemap bounds
            if (!bounds.Contains(cellPos))
                continue;

            // 4️⃣ Check floor tile exists
            if (!floorTilemap.HasTile(cellPos))
                continue;

            // 5️⃣ Convert back to cell center
            Vector3 worldPos = floorTilemap.GetCellCenterWorld(cellPos);

            // 6️⃣ Optional: prevent on-screen spawning
            Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
            if (viewPos.x > 0 && viewPos.x < 1 &&
                viewPos.y > 0 && viewPos.y < 1)
                continue;

            return worldPos;
        }

        // Fallback (rare)
        return floorTilemap.GetCellCenterWorld(bounds.position);
    }
}
