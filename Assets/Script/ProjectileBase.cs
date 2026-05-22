using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float _speed = 20f;
    [SerializeField] protected float _lifeTime = 5f;

    protected Vector3 _moveDirection;

    protected virtual void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    public virtual void Initialize(Vector3 direction)
    {
        _moveDirection = direction.normalized;
    }

    protected virtual void Update()
    {
        transform.position +=
            _moveDirection * _speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
