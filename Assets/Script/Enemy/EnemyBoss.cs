using UnityEngine;
using UnityEngine.Timeline;
using System;
using System.Collections;

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
    [SerializeField] private AsteroidProjectile _asteroidPrefab;
    [SerializeField] private HoundProjectile _houndPrefab;
    [SerializeField] private ViperProjectile _viperPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _attackInterval = 2f;
    [Header("Sheld")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Transform _shieldPoint;
    [SerializeField] private float _shieldDuration = 3f;
    [SerializeField] private float _shieldCooldown = 5f;
    [Header("BigCube")]
    [SerializeField] private GameObject _bigCubePrefab;
    [SerializeField] private float _bigCubeDelay = 0.3f;

    [Header("Distance")]
    [SerializeField] private float _attackRange = 20f;
    [SerializeField] private float _phase2Distance = 10f;

    private Transform _aimPoint;

    private float _attackTimer;
    private bool _phase2;

    private Vector3 _startPosition;
    private float _returnTimer;
    private BossState _state;
    private CharacterController _characterController;
    private Animator _animator;

    private GameObject _shield;
    private bool _canShield = true;
    private float _shieldTimer;
    private float _cooldownTimer;
    private bool _isAttacking;

    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _aimPoint = _player.Find("EnemyAimPoint");
        _startPosition = transform.position;
        _state = BossState.idle;
        _characterController = GetComponent<CharacterController>();
        _maxHP = _hp;
        ShooterWeponBase.OnPlayerShot += OnPlayerShot;
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        if (_player == null)
        {
            return;
        }

        CheckPhase();
        if (_state != BossState.Return)
        {
            LookPlayer();
        }

        if (_state == BossState.Shield)
        {
            UpdateShield();
            return;
        }

        Attack();
        Move();
        UpdateCooldown();
        UpdateLockOnMarker();
        Debug.Log(_state);
    }

    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    private void LookPlayer()
    {
        if (_state == BossState.Return)
        {
            return;
        }

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

        // 初期位置にいてプレイヤーも遠いなら待機
        if (playerDistance > _attackRange && startDistance < 0.2f)
        {
            _state = BossState.idle;
            _returnTimer = 0f;
            _animator.SetBool("Move", false);
            return;
        }

        //攻撃範囲外の場合
        if (playerDistance > _attackRange)
        {
            _returnTimer += Time.deltaTime;

            if( _returnTimer >= _returnTime)
            {
                _state = BossState.Return;
                _returnTimer = 0f;
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
                    _animator.SetBool("Move", true);

                    Vector3 dir = (_player.position - transform.position).normalized;
                    _characterController.Move(dir * _moveSpeed * Time.deltaTime);
                    break;
                }

            case BossState.Return:
                {
                    _animator.SetBool("Move", true);
                    Vector3 dir = (_startPosition - transform.position).normalized;

                    // 帰還中は初期位置を見る
                    if (dir != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(dir);
                    }

                    _characterController.Move(dir * _moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, _startPosition) < 0.2f)
                    {
                        _state = BossState.idle;
                    }
                    break;
                }

            default:
                {
                    _animator.SetBool("Move", false);
                    break;
                }
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        if (_state != BossState.Attack)
        {
            return;
        }

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
            StartCoroutine(ShotRoutine(FireAsteroid));
            return;
        }

        // 後半

        if (distance >= _phase2Distance)
        {
            if (UnityEngine.Random.value < 0.7f)
            {
                StartCoroutine(ShotRoutine(FireHound));
            }
            else
            {
                StartCoroutine(ShotRoutine(FireViper));
            }
        }
        else
        {
            if (UnityEngine.Random.value < 0.7f)
            {
                StartCoroutine(ShotRoutine(FireAsteroid));
            }
            else
            {
                StartCoroutine(ShotRoutine(FireViper));
            }
        }
    }

    private IEnumerator ShotRoutine(Action fireAction)
    {
        _isAttacking = true;
        _animator.SetTrigger("BigCube");

        // 手を前に出すまで待つ
        yield return new WaitForSeconds(0.3f);

        GameObject bigCube = Instantiate(
            _bigCubePrefab,
            _firePoint.position,
            Quaternion.identity);

        // 大玉を保持
        yield return new WaitForSeconds(_bigCubeDelay);

        SplitAndFire(bigCube, fireAction);

        Destroy(bigCube);
       
    }

    private void SplitAndFire(GameObject bigCube, Action fireAction)
    {
        Vector3[] offsets =
        {
        new Vector3(0, 0.3f, 0),
        new Vector3(-0.3f, 0, 0),
        new Vector3(0.3f, 0, 0),
        new Vector3(0, -0.3f, 0)
    };
        _animator.SetTrigger("Shoot");
        Vector3 originalPosition = _firePoint.position;

        for (int i = 0; i < offsets.Length; i++)
        {
            _firePoint.position = bigCube.transform.position + offsets[i];
            fireAction?.Invoke();
        }

        _firePoint.position = originalPosition;

    }

    private IEnumerator DelayFire(ProjectileBase projectile, Vector3 direction)
    {
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        projectile.Initialize(direction);
    }

    private void FireAsteroid()
    {
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;

        AsteroidProjectile bullet =
            Instantiate(_asteroidPrefab,
            _firePoint.position,
            Quaternion.LookRotation(dir));

        StartCoroutine(DelayFire(bullet, dir));
    }

    private void FireHound()
    {
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;

        HoundProjectile bullet =
            Instantiate(_houndPrefab,
            _firePoint.position,
            Quaternion.LookRotation(dir));

        bullet.SetTarget(_aimPoint);
        StartCoroutine(DelayFire(bullet, dir));
    }

    private void FireViper()
    {
        Vector3 dir = (_aimPoint.position - _firePoint.position).normalized;

        ViperProjectile bullet =
            Instantiate(_viperPrefab,
            _firePoint.position,
            Quaternion.LookRotation(dir));

        bullet.SetPattern(ViperProjectile.ViperPattern.SideCurve);
        bullet.SetTargetPosition(_aimPoint.position);
        StartCoroutine(DelayFire(bullet, dir));
    }

    private void ActivateShield()
    {
        if (!_canShield)
        {
            return;
        }

        _animator.SetBool("Move", false);
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

        if (_isAttacking)     // ←追加
            return;

        ActivateShield();
    }

    private void OnDestroy()
    {
        ShooterWeponBase.OnPlayerShot -= OnPlayerShot;
    }
}


