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
        Debug.Log("Initialize");
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
        EnemyBase enemy =
        other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}
