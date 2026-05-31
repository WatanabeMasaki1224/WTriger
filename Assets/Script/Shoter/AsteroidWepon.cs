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

    private IEnumerator SetAsteroidDirection(AsteroidProjectile projectile, Vector3 dir)
    {
        yield return new WaitForSeconds(0.2f);

        projectile.Initialize(dir);
    }
}
