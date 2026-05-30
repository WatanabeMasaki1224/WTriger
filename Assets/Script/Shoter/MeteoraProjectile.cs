using UnityEngine;

public class MeteoraProjectile : ProjectileBase
{
    [Header("ÉÅÉeÉIÉâ")]
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _explosionDamage = 40f;

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name + " / layer:" + other.gameObject.layer);
        Exprode();
    }

    private void Exprode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyBase>(out var enemy))
            {
                enemy.TakeDamage(_explosionDamage);
                Destroy(gameObject);
            }
        }

        
    }
}
