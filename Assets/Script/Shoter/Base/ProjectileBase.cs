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

    protected Vector3 _moveDirection;
    protected bool _canMove;

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

        transform.position +=
            _moveDirection * _speed * Time.deltaTime;
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) return;

        if (other.TryGetComponent<ProjectileBase>(out _))
            return;

        // 着弾エフェクト生成
        if (_hitEffectPrefab != null)
        {
            Instantiate(_hitEffectPrefab, transform.position,Quaternion.identity);
        }

        if (other.TryGetComponent<EnemyBase>(out var enemy))
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }

        // それ以外は全部消す（壁・シールド・地形など）
        Destroy(gameObject);
    }
}
