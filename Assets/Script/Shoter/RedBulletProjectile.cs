using UnityEngine;

public class RedBulletProjectile : AsteroidProjectile
{
    [SerializeField] private GameObject _redBulletEffectPrefab;
    protected override void OnTriggerEnter(Collider other)
    {
        // 弾同士は当たり判定を無視
        if (other.TryGetComponent<ProjectileBase>(out _))
        {
            return;
        }

        // 敵に命中
        if (other.TryGetComponent<EnemyBase>(out var enemy))
        {
            // デバフ
            enemy.ApplyRedBullet();
            Instantiate(_redBulletEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        // シールドに命中
        if (other.TryGetComponent<ShieldObject>(out var shield))
        {
            return;
        }

        // それ以外
        Destroy(gameObject);
    }
}
