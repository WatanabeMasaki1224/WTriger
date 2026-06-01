using UnityEngine;
using System.Collections;

public class MeteoraWepon : ShooterWeponBase
{
    [Header("Meteora")]
    [SerializeField] private MeteoraProjectile _meteoraPrefab;
    protected override IEnumerator ShootRoutine()
    {
        _isShooting = true;

        // ëÂã ê∂ê¨
        GameObject bigCube = Instantiate(
            _bigCubePrefab,
            _firePoint.position,
            Quaternion.identity
        );

        // è≠Çµë“ã@
        yield return new WaitForSeconds(_cubeDelay);

        CreateProjectile(bigCube.transform.position);
        Destroy(bigCube );
        _isShooting = false;
    }

    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        Transform target = GetLockTarget();

        Vector3 dir = _cameraTransform.forward;

        if (target != null)
        {
            dir = (target.position - spawnPosition).normalized;
        }

        MeteoraProjectile projectile = Instantiate(
            _meteoraPrefab,
            spawnPosition,
            Quaternion.identity
        );
        StartCoroutine(SetAsteroidDirection(projectile,dir));
    }

    private IEnumerator SetAsteroidDirection(MeteoraProjectile projectile, Vector3 dir)
    {
        yield return new WaitForSeconds(0.2f);

        projectile.Initialize(dir);
    }
}
