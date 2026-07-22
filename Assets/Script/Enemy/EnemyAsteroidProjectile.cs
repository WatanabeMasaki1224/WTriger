using UnityEngine;

public class EnemyAsteroidProjectile : ProjectileBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) return;

        if (other.TryGetComponent<ProjectileBase>(out _))
            return;

        if (other.TryGetComponent<PlayerStatus>(out var player))
        {
            player.TakeDamage(_damage);
            Destroy(gameObject);
        }

        // それ以外は全部消す（壁・シールド・地形など）
        Destroy(gameObject);
    }
}
