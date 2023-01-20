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

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }


    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDie)
        {
            Die();
        }

    }

    [ContextMenu("Self Destruct")]
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
