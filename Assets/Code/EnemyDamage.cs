using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float health;
    public float maxHealth;

    [SerializeField] HealthBar HB;

    private void Awake()
    {
        HB = GetComponentInChildren<HealthBar>();  
    }

    public void Start()
    {
        health = maxHealth;
        HB.UpdateHealthBar(health, maxHealth);
    }

    public void DealDamage(float damageAmount)
    {
        health -= damageAmount;
        HB.UpdateHealthBar(health, maxHealth);
        CheckOverheal(); // Call the method to handle overhealing
        CheckDeath();
    }

    private void CheckOverheal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
