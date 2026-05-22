using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShooterWeponBase : MonoBehaviour
{
    [Header("参考")]
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Transform _cameraTransform;
    [SerializeField] protected GameObject _bigCubePrefab;
    [SerializeField] protected GameObject _projectilePrefab;
    [Header("ステータス")]
    [SerializeField] protected int _cubeCount = 4;
    [SerializeField] protected float _cubeDelay = 0.3f;
    [SerializeField] protected float _cubeSpread = 0.3f;
    protected bool isShooting;

    public void OnFire(InputAction.CallbackContext context)
    {
        if (isShooting)
        {
            return;
        }

        StartCoroutine(ShootRoutine());
    }

    protected virtual IEnumerator ShootRoutine()
    {
        isShooting = true;

        // 大玉生成
        GameObject bigCube = Instantiate(
            _bigCubePrefab,
            _firePoint.position,
            Quaternion.identity
        );

        // 少し待機
        yield return new WaitForSeconds(_cubeDelay);

        // 分割＆発射
        SplitAndFire(bigCube);

        isShooting = false;
    }

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

            // 小弾生成
            GameObject projectile = Instantiate(
                _projectilePrefab,
                spawnPosition,
                Quaternion.LookRotation(_cameraTransform.forward)
            );

            // ProjectileBase取得
            ProjectileBase projectileBase =
                projectile.GetComponent<ProjectileBase>();

            // 発射方向設定
            if (projectileBase != null)
            {
                projectileBase.Initialize(_cameraTransform.forward);
            }
        }

        // 大玉削除
        Destroy(bigCube);
    }
}
