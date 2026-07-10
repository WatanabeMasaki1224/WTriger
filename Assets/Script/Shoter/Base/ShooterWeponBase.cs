using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterWeponBase : MonoBehaviour
{
    [Header("参考")]
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Transform _cameraTransform;
    [SerializeField] protected GameObject _bigCubePrefab;
    [SerializeField] protected AsteroidProjectile _asteroidPrefab;
    [Header("ステータス")]
    [SerializeField] protected int _cubeCount = 4;
    [SerializeField] protected float _cubeDelay = 0.3f;
    [SerializeField] protected float _cubeSpread = 0.5f;
    [SerializeField] protected float _trionCost = 10f;
    [Header("ロックオン")]
    [SerializeField] protected float _lockOnRange = 15f;
    [SerializeField] protected float _lookOnAngle = 60f;
    [SerializeField] protected LayerMask _enemyLayer;
    [Header("SE")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _bigCubeSE;
    [SerializeField] private AudioClip _shotSE;

    protected PlayerStatus _playerStatus;
    protected bool _isShooting;
    public bool IsShooting => _isShooting;
    protected Animator _animator;

    public static event Action OnPlayerShot;

    protected virtual void Start()
    {
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 発射入力を受け取る
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (_isShooting)
        {
            return;
        }

        if (!_playerStatus.ConsumeTrion(_trionCost))
        {
            return;
        }
        _animator.SetTrigger("BigCube");

        StartCoroutine(ShootRoutine());
    }

    /// <summary>
    /// 大玉を作成、分割し発射までの流れ
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator ShootRoutine()
    {
        _isShooting = true;
        yield return new WaitForSeconds(0.3f);
        // 大玉生成
        GameObject bigCube = Instantiate(
            _bigCubePrefab,
            _firePoint.position,
            Quaternion.identity
        );

        _audioSource.PlayOneShot(_bigCubeSE);
        // 少し待機
        yield return new WaitForSeconds(_cubeDelay);

        // 分割＆発射
        SplitAndFire(bigCube);

        yield return new WaitForSeconds(0.5f);

        _isShooting = false;
    }

    /// <summary>
    /// 大玉の分割処理
    /// </summary>
    /// <param name="bigCube"></param>
    protected virtual void SplitAndFire(GameObject bigCube)
    {
        Vector3[] offsets =
        {
            new Vector3(0, 0.3f, 0), 
            new Vector3(-0.3f, 0, 0), 
            new Vector3(0.3f, 0, 0),
            new Vector3(0, -0.3f, 0)
        };

        for (int i = 0; i < _cubeCount; i++)
        {
            Vector3 spawnPosition = bigCube.transform.position + offsets[i];
            CreateProjectile(spawnPosition);
        }

        // ★プレイヤーが攻撃したことを通知
        OnPlayerShot?.Invoke();
        _animator.SetTrigger("Shoot");
        _audioSource.PlayOneShot(_shotSE);
        // 大玉削除
        Destroy(bigCube);
    }

    /// <summary>
    /// 弾を発射
    /// </summary>
    /// <param name="projectileBase"></param>
    /// <returns></returns>
    protected virtual IEnumerator FireProjectile(ProjectileBase projectileBase)
    {
        _animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f);

        projectileBase.Initialize(_cameraTransform.forward);
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    /// <param name="spawnPosition"></param>
    protected virtual void CreateProjectile(Vector3 spawnPosition)
    {
        AsteroidProjectile projectile = Instantiate(
            _asteroidPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );

        StartCoroutine(FireProjectile(projectile));
    }

    /// <summary>
    /// ロックの対象を所得
    /// </summary>
    /// <returns></returns>
    protected Transform GetLockTarget()
    {
        Collider[] hits = Physics.OverlapSphere(_firePoint.position, _lockOnRange, _enemyLayer);
        Debug.Log($"候補数:{hits.Length}");
        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Debug.Log(hit.name);
            Vector3 enemyPosition = hit.transform.position - _firePoint.position;
            float angle = Vector3.Angle(_cameraTransform.forward, enemyPosition);

            if(angle > _lookOnAngle * 0.5f)
            {
                continue;
            }

            float distance  =  enemyPosition.magnitude;

            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = hit.transform;
            }
                
        }

        return nearestTarget;
    }
}
