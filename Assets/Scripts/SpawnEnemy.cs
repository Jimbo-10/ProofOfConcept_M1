using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnEnemy : MonoBehaviour
{
    public Transform player;
    public Tilemap tileMap;
    public GameObject enemyPrefab;
    public GameObject zombiePrefab;

    public float minSpawnDistance = 4f;
    public float maxSpawnDistance = 8f;
    public float spawnInterval = 2f;
    public int maxEnemies = 20;

    private BoundsInt bounds;

    public int wave = 1;
    public int enemiesPerWave = 5;

    private int enemiesSpawned;

    public float waveDuration = 10f;

    private float waveTimer;
    private float spawnTimer;

    UIManager uiManager;

    void Start()
    {
        bounds = tileMap.cellBounds;
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Spawn enemies
        if (spawnTimer >= spawnInterval && enemiesSpawned < enemiesPerWave)
        {
            EnemySpawn();
            enemiesSpawned++;
            spawnTimer = 0f;
        }

        // End wave
        if (waveTimer >= waveDuration)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        wave++;
        waveTimer = 0f;
        enemiesSpawned = 0;

        // Increase difficulty
        enemiesPerWave += 3; // more enemies each wave
        spawnInterval = Mathf.Max(0.4f, spawnInterval - 0.2f); // faster spawn

        Debug.Log("Wave " + wave);
    }
    void EnemySpawn()
    {
        Vector3 spawnPos = GetValidSpawnPosition();
        GameObject newEnemy;

        if (uiManager.GetCurrentTime() <= 30f)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
                newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            else
                newEnemy = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }

        // Make Enemy 2 stronger
        EnemyStats stats = newEnemy.GetComponent<EnemyStats>();
        if (stats != null)
        {
            if (newEnemy == zombiePrefab)  // or use tag
            {
                stats.maxHealth = 100f;
                stats.damage = 20;
            }
            else
            {
                stats.maxHealth = 50f;
                stats.damage = 50;
            }
            stats.currentHealth = stats.maxHealth;
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int i = 0; i < 50; i++) 
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector2 candidatePos = (Vector2)player.position + direction * distance;

            
            Vector3Int cellPos = tileMap.WorldToCell(candidatePos);
            
            if (!bounds.Contains(cellPos))
                continue;
           
            if (!tileMap.HasTile(cellPos))
                continue;
           
            Vector3 worldPos = tileMap.GetCellCenterWorld(cellPos);

            Vector3 viewPos = Camera.main.WorldToViewportPoint(worldPos);
            if (viewPos.x > 0 && viewPos.x < 1 &&
                viewPos.y > 0 && viewPos.y < 1)
                continue;

            return worldPos;
        }

        return tileMap.GetCellCenterWorld(bounds.position);
    }
}
