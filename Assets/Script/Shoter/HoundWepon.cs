using UnityEngine;

public class HoundWepon : ShooterWeponBase
{ 
    [Header("Hound")]
    [SerializeField] private HoundProjectile _houndPrefab;

    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        Transform target = GetLockTarget();

        HoundProjectile projectile = Instantiate(
            _houndPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );

        projectile.SetTarget(target);
        StartCoroutine(FireProjectile(projectile));
    }
}
