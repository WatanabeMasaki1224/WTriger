using UnityEngine;
using UnityEngine.Timeline;

public class EnemyBoss : EnemyBase
{
    public enum BossState
    {
        idle,
        Attack,
        Chase,
        Return,
        Shield
    }

    [Header("Move")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _chaseDistance = 15f;    //近づける距離
    [SerializeField] private float _moveDistance = 10f;          //初期位置から動ける距離
    [SerializeField] private float _returnTime = 3f;         //戻り始めるまでの時間
    [Header("Attack")]
    [SerializeField] private EnemyAsteroidProjectile _asteroidPrefab;
    [SerializeField] private HoundProjectile _houndPrefab;
    [SerializeField] private ViperProjectile _viperPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _attackInterval = 2f;
    [Header("Sheld")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Transform _shieldPoint;
    [SerializeField] private float _shieldDuration = 3f;
    [SerializeField] private float _shieldCooldown = 5f;

    [Header("Distance")]
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _phase2Distance = 10f;

    private Transform _player;
    private Transform _aimPoint;

    private float _attackTimer;
    private bool _phase2;

    private Vector3 _startPosition;
    private float _returnTimer;
    private BossState _state;
    private CharacterController _characterController;

    private GameObject _shield;
    private bool _canShield = true;
    private float _shieldTimer;
    private float _cooldownTimer;

    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aimPoint = _player.Find("EnemyAimPoint");
        _startPosition = transform.position;
        _state = BossState.idle;
        _characterController = GetComponent<CharacterController>();
        _maxHP = _hp;
        ShooterWeponBase.OnPlayerShot += OnPlayerShot;
    }

    protected override void Update()
    {
        if (_player == null)
        {
            return;
        }

        LookPlayer();
        CheckPhase();

        if (_state == BossState.Shield)
        {
            UpdateShield();
            return;
        }

        Attack();
        Move();
        UpdateCooldown();
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
        if (!_phase2 && _hp <= _maxHP/2)
        {
            _phase2 = true;
            Debug.Log("Phase2");
        }
    }

    protected override void Move()
    {
        float playerDistance = Vector3.Distance(transform.position, _player.position);
        float startDistance = Vector3.Distance(transform.position,_startPosition);

        //攻撃範囲外の場合
        if(playerDistance > _attackRange)
        {
            _returnTimer += Time.deltaTime;

            if( _returnTimer >= _returnTime)
            {
                _state = BossState.Return;
            }
        }
        else
        {
            _returnTimer = 0;

            if (playerDistance > _chaseDistance && startDistance < _moveDistance)
            {
                _state = BossState.Chase;
            }
            else
            {
                _state = BossState.Attack;
            }
        }

        // ここで状態ごとの処理
        switch (_state)
        {
            case BossState.Chase:
                {
                    Vector3 dir = (_player.position - transform.position).normalized;
                    _characterController.Move(dir * _moveSpeed * Time.deltaTime);
                    break;
                }

            case BossState.Return:
                {
                    Vector3 dir = (_startPosition - transform.position).normalized;
                    _characterController.Move(dir * _moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, _startPosition) < 0.2f)
                    {
                        _state = BossState.idle;
                    }
                    break;
                }
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

    private void ActivateShield()
    {
        if (!_canShield)
        {
            return;
        }

        _shield = Instantiate(_shieldPrefab, _shieldPoint.position, transform.rotation, transform);

        _state = BossState.Shield;
        _shieldTimer = 0f;
        _canShield = false;
    }

    private void UpdateShield()
    {
        _shieldTimer += Time.deltaTime;

        if (_shieldTimer < _shieldDuration)
        {
            return;
        }

        Destroy(_shield);

        _shield = null;
        _state = BossState.idle;

        _cooldownTimer = 0f;
    }

    private void UpdateCooldown()
    {
        if (_canShield)
        {
            return;
        }

        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _shieldCooldown)
        {
            _canShield = true;
        }
    }

    private void OnPlayerShot()
    {
        // シールド中なら何もしない
        if (_state == BossState.Shield)
        {
            return;
        }

        // クールタイム中なら何もしない
        if (!_canShield)
        {
            return;
        }

        ActivateShield();
    }

    private void OnDestroy()
    {
        ShooterWeponBase.OnPlayerShot -= OnPlayerShot;
    }
}


