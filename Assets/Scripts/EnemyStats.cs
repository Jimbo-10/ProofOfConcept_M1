using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth = 50f;   // default health
    public float currentHealth;
    
    public int damage = 10;         // damage to player

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // optionally tell UIManager to add kill count
    }
}
