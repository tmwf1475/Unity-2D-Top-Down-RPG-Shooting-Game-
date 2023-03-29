using UnityEngine;

public class WoodCrate : LivingEntity
{
    public override void TakeDamage(float damage)
    {
        if (damage >= Health && !IsDead)
        {
            OnDeath += () => {Debug.Log("wood crate die !");};
        }

        //dead effect
        //dead sound
        base.TakeDamage(damage);
    }
}
