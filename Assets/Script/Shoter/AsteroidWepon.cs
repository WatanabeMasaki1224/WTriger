using UnityEngine;
using System.Collections;

public class AsteroidWepon : ShooterWeponBase
{
    /// <summary>
    /// アステロイドの弾を生成する
    /// </summary>
    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        // ロックオン対象を取得
        Transform target = GetLockTarget();

        // ロックオン対象がいる場合は敵の方向へ発射
        Vector3 dir = _cameraTransform.forward;

        if (target != null)
        {
            dir = (target.position - spawnPosition).normalized;
        }

        AsteroidProjectile projectile = Instantiate(
            _asteroidPrefab,
            spawnPosition,
            Quaternion.identity
        );

        StartCoroutine(SetAsteroidDirection(projectile, dir));
    }

    /// <summary>
    ///  発射アニメーション後に弾を発射する
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private IEnumerator SetAsteroidDirection(AsteroidProjectile projectile, Vector3 dir)
    {
        _animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f);

        projectile.Initialize(dir);
    }
}
