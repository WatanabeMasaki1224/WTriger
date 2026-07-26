using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _hp = 100f;
    private EnemySpawner _spawner;
    protected float _maxHP;
    [SerializeField] private GameObject _lockOnMarker;
    [SerializeField] private float _markerDistance = 15f;
    protected Transform _player;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSE;
    [SerializeField] private GameObject _bailoutPrefab;
    [SerializeField] private GameObject _deadEffectPrefab;
    [SerializeField] private float _bailoutDelay = 0.4f;
    [Header("Move")]
    [SerializeField] protected float _moveSpeed = 3f;

    [Header("Attack")]
    [SerializeField] protected float _attackInterval = 2f;
    [Header("RedBullet")]
    [SerializeField] private float _redBulletMoveRate = 0.5f;      // 移動速度倍率
    [SerializeField] private float _redBulletAttackRate = 2f;      // 攻撃間隔倍率
    [SerializeField] private float _redBulletDuration = 5f;

    protected float _defaultMoveSpeed;
    protected float _defaultAttackInterval;

    private Coroutine _redBulletCoroutine;

    private bool _isDead;
    public float HP => _hp;
    public float MaxHP => _maxHP;

    protected virtual void Start()
    {
        _maxHP = _hp;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _defaultMoveSpeed = _moveSpeed;
        _defaultAttackInterval = _attackInterval;

    }

    protected virtual void Update()
    {
        UpdateLockOnMarker();
    }

    protected virtual void Move()
    {

    }

    protected virtual void Attack()
    {

    }

    public virtual void ApplyRedBullet()
    {
        if (_redBulletCoroutine != null)
        {
            StopCoroutine(_redBulletCoroutine);
        }

        _redBulletCoroutine = StartCoroutine(RedBulletRoutine());
    }

    private IEnumerator RedBulletRoutine()
    {
        _moveSpeed = _defaultMoveSpeed * _redBulletMoveRate;
        _attackInterval = _defaultAttackInterval * _redBulletAttackRate;

        yield return new WaitForSeconds(_redBulletDuration);

        _moveSpeed = _defaultMoveSpeed;
        _attackInterval = _defaultAttackInterval;

        _redBulletCoroutine = null;
    }

    public virtual void TakeDamage(float damage)
    {
        if (_isDead)
        {
            return;
        }

        _hp -= damage;
        _audioSource.PlayOneShot(_hitSE);

        if (_hp <= 0)
        {
            _isDead = true;
            Die();
        }
        else
        {
            StartCoroutine(HitStop());
        }
    }

    /// <summary>
    /// ヒットストップを行う
    /// </summary>
    private IEnumerator HitStop()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1f;
    }

    /// <summary>
    /// 敵の死亡処理
    /// </summary>
    protected virtual void Die()
    {

        Time.timeScale = 1f;
        // キル数を加算
        FindObjectOfType<GameManager>().AddKillCount();

        // モデルを非表示にする
        foreach (var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = false;
        }

        // スポナーへ死亡を通知
        if (_spawner != null)
        {
            _spawner.EnemyDead(this);
        }
        StartCoroutine(DeadRoutine());
    }

    /// <summary>
    /// 死亡演出(ベイルアウト)を再生する
    /// </summary>
    private IEnumerator DeadRoutine()
    {
        Instantiate(_deadEffectPrefab,transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_bailoutDelay);
        Instantiate(_bailoutPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    /// <summary>
    /// スポナーを設定する
    /// </summary>
    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    /// <summary>
    /// ロックオンマーカーの表示を更新する
    /// </summary>
    protected virtual void UpdateLockOnMarker()
    {
        if (_player == null || _lockOnMarker == null)
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, _player.position);

        _lockOnMarker.SetActive(distance <= _markerDistance);
    }
}
