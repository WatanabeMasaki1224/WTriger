using UnityEditor;
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    [Header("Move")] 
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _stopDistance = 5f;
    [Header("Attack")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _attackCooldown = 2f;

    private Transform _player;
    private float _attackTimer;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        Move();
        Attack();
    }

    protected override void Move()
    {
        if(_player != null)
        {
            return;
        }

        float distance =Vector3.Distance(transform.position,_player.position);
        //‹——£‚ªˆê’è‹——£‰“‚¢ê‡‹ß‚Ã‚­
        if(distance > _stopDistance)
        {
            Vector3 dir = (_player.position - transform.position).normalized;
            transform.position += dir * _moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// UŒ‚‚ÌƒN[ƒ‹ƒ^ƒCƒ€‚ª‚ ‚¯‚½‚çFire‚ğ‚æ‚ÑUŒ‚
    /// </summary>
    private void Attack()
    {
        if(_player != null)
        {
            return;
        }

        _attackTimer += Time.deltaTime;


        if (_attackTimer < _attackCooldown)
        {
            return;
        }


        _attackTimer = 0f;

        Fire();
    }

    /// <summary>
    /// UŒ‚
    /// </summary>
    private void Fire()
    {
        Vector3 dir = (_player.position - _firePoint.position).normalized;
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        //
        bullet.GetComponent<EnemyAsteroidProjectile>().Initialize(dir);
    }
}
