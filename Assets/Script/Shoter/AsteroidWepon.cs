using UnityEngine;
 using System.Collections;

public class AsteroidWepon : ShooterWeponBase
{
    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        Transform target = GetLockTarget();

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
    ///  弾を発射（引数の関係でベースのやつがつかえないため）
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
