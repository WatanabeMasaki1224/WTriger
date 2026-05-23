using UnityEngine;

public class HoundWepon : ShooterWeponBase
{ 
    [Header("Hound")]
    [SerializeField] private HoundProjectile _houndPrefab;

    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        GameObject enemy =
            GameObject.FindGameObjectWithTag("Enemy");
        Debug.Log(enemy);
        Transform enemyTransform = null;

        if (enemy != null)
        {
            enemyTransform = enemy.transform;
        }

        HoundProjectile projectile = Instantiate(
            _houndPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );

        projectile.SetTarget(enemyTransform);

        StartCoroutine(FireProjectile(projectile));
    }
}
