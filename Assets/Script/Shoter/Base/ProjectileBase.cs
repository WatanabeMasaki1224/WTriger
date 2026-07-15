using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float _speed = 20f;
    [SerializeField] protected float _lifeTime = 5f;
    [SerializeField] protected float _damage = 20f;
    [Header("Effects")]
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private GameObject _shieldHitEffectPrefab;

    protected Vector3 _moveDirection;
    protected bool _canMove;

    /// <summary>
    /// 弾の寿命を設定する
    /// </summary>
    protected virtual void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
    /// <summary>
    /// 弾の移動方向を設定する
    /// </summary>
    /// <param name="direction"></param>
    public virtual void Initialize(Vector3 direction)
    {
        _moveDirection = direction.normalized;
        _canMove = true;
    }

    /// <summary>
    /// 弾を移動させる
    /// </summary>
    protected virtual void Update()
    {
        if (!_canMove)
        {
            return;
        }

        transform.position += _moveDirection * _speed * Time.deltaTime;
    }

    /// <summary>
    /// オブジェクトと衝突した時の処理
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // 弾同士は当たり判定を無視
        if (other.TryGetComponent<ProjectileBase>(out _))
        {
            return;
        }

        // 敵に命中
        if (other.TryGetComponent<EnemyBase>(out var enemy))
        {
            enemy.TakeDamage(_damage);
            Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        // シールドに命中
        if (other.TryGetComponent<ShieldObject>(out var shield))
        {
            Instantiate(_shieldHitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        // プレイヤーに命中
        if (other.TryGetComponent<PlayerStatus>(out var player))
        {
            player.TakeDamage(_damage);
            Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }


        // それ以外
        Destroy(gameObject);
    }
}
