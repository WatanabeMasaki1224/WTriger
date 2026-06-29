using UnityEngine;

public class EnemyBoss : EnemyBase
{
    [Header("Attack")]
    [SerializeField] private EnemyAsteroidProjectile _asteroidPrefab;
    [SerializeField] private HoundProjectile _houndPrefab;
    [SerializeField] private ViperProjectile _viperPrefab;
    [SerializeField] private Transform _firePoint;

    [Header("Distance")]
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _phase2Distance = 10f;

    [Header("Attack")]
    [SerializeField] private float _attackInterval = 2f;

    private Transform _player;
    private Transform _aimPoint;

    private float _attackTimer;
    private bool _phase2;

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aimPoint = _player.Find("EnemyAimPoint");
    }

    protected override void Update()
    {
        if (_player == null)
        {
            return;
        }

        LookPlayer();
        CheckPhase();
        Attack();
    }

    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    private void LookPlayer()
    {
        Vector3 dir = _player.position - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    /// <summary>
    /// HP50%以下で第2フェーズ
    /// </summary>
    private void CheckPhase()
    {
        if (!_phase2 && _hp <= 50f)
        {
            _phase2 = true;
            Debug.Log("Phase2");
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance > _attackRange)
        {
            return;
        }

        _attackTimer += Time.deltaTime;

        if (_attackTimer < _attackInterval)
        {
            return;
        }

        _attackTimer = 0f;

        // 前半
        if (!_phase2)
        {
            FireAsteroid();
            return;
        }

        // 後半

        if (distance >= _phase2Distance)
        {
            // 10m～20m

            if (Random.value < 0.7f)
            {
                FireHound();
            }
            else
            {
                FireViper();
            }
        }
        else
        {
            // 10m未満

            if (Random.value < 0.7f)
            {
                FireAsteroid();
            }
            else
            {
                FireViper();
            }
        }
    }

    private void FireAsteroid()
    {
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;

        EnemyAsteroidProjectile bullet =
            Instantiate(_asteroidPrefab,
            _firePoint.position,
            Quaternion.LookRotation(dir));

        bullet.Initialize(dir);
    }

    private void FireHound()
    {
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;

        HoundProjectile bullet =
            Instantiate(_houndPrefab,
            _firePoint.position,
            Quaternion.LookRotation(dir));

        bullet.Initialize(dir);
        bullet.SetTarget(_aimPoint);
    }

    private void FireViper()
    {
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;

        ViperProjectile bullet =
            Instantiate(_viperPrefab,
            _firePoint.position,
            Quaternion.LookRotation(dir));

        bullet.Initialize(dir);
        bullet.SetPattern(ViperProjectile.ViperPattern.SideCurve);
        bullet.SetTargetPosition(_aimPoint.position);
    }
}
