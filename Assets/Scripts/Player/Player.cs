public class Player : LivingEntity
{
    protected override void Start()
    {
        base.Start();

        GameEvents.OnPlayerHealthChanged.Invoke(Health, startHealth);
    }
    
    public override void TakeDamage(float damage)
    {
        if (damage >= Health && !IsDead)
        {
            GameEvents.GameOver?.Invoke();
        }
        
        base.TakeDamage(damage);

        GameEvents.OnPlayerHealthChanged.Invoke(Health, startHealth);
    }
}
