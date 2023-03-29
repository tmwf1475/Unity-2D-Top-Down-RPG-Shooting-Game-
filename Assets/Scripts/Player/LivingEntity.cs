using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHealth;
    protected float Health
    {
        get; 
        private set;
    }
    protected bool IsDead;

    public  ParticleSystem deathParticle;

    public event Action OnDeath;

    protected virtual void Start() 
    {
        Health = startHealth;
    }
    
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health > 0 || IsDead)
        {
            return;
        }

        IsDead = true;
        OnDeath?.Invoke();

        Destroy(gameObject);

        // Death 
        if (deathParticle == null)
        {
            Debug.LogError($"Can't find death particle on {gameObject.name}");
            return;
        }
        ParticleSystem ps = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(ps.gameObject,  ps.main.duration);
    }
}

