
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    [Header("Move")] 
    [SerializeField] private float _stopDistance = 5f;
    [Header("Attack")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _attackCooldown = 2f;

    private Transform _aimPoint;
    private float _attackTimer;

    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aimPoint = _player.Find("EnemyAimPoint");
    }

    protected override void Update()
    {
        Move();
        Attack();
    }

    protected override void Move()
    {
        if(_player == null)
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
    protected override void Attack()
    {
        if(_player == null)
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
        Debug.DrawLine(
      _firePoint.position,
      _aimPoint.position,
      Color.red,
      2f
  );
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        //
        bullet.GetComponent<EnemyAsteroidProjectile>().Initialize(dir);
    }
}
