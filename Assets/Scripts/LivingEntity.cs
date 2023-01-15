using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool isDie;

    public event System.Action OnDeath;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = startingHealth;
    }

 
    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDie)
        {
            Die();
        }

    }

    protected void Die()
    {
        isDie = true;

        if(OnDeath != null)
        {
            OnDeath();
        }

        Destroy(gameObject);
    }
}
