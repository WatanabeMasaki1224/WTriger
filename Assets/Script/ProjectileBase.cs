using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float _speed = 20f;
    [SerializeField] protected float _lifeTime = 5f;
    [SerializeField] protected float _damage = 20f;

    protected Vector3 _moveDirection;
    protected bool _canMove;

    protected virtual void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    public virtual void Initialize(Vector3 direction)
    {
        _moveDirection = direction.normalized;
        _canMove = true;
    }

    protected virtual void Update()
    {
        if (!_canMove)
        {
            return;
        }

        transform.position +=
            _moveDirection * _speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) return;

        if (other.TryGetComponent<ProjectileBase>(out _))
            return;

        if (other.TryGetComponent<EnemyBase>(out var enemy))
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
