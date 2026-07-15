using UnityEngine;

public class HoundWepon : ShooterWeponBase
{ 
    [Header("Hound")]
    [SerializeField] private HoundProjectile _houndPrefab;

    /// <summary>
    /// 弾を生成し　ターゲットを設定
    /// </summary>
    /// <param name="spawnPosition"></param>
    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        // ロックオン対象を取得
        Transform target = GetLockTarget();

        HoundProjectile projectile = Instantiate(
            _houndPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );

        // ターゲットを設定して発射
        projectile.SetTarget(target);
        StartCoroutine(FireProjectile(projectile));
    }
}
