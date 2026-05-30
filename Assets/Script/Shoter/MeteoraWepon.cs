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
        MeteoraProjectile projectile = Instantiate(
            _meteoraPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );
        StartCoroutine(FireProjectile(projectile));
    }
}
