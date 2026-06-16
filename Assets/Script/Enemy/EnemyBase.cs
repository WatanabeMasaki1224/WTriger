using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _hp = 100f;

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        _hp -= damage;
        Debug.Log($"EnemyDmage:{damage} ");

        if( _hp <= 0 )
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
