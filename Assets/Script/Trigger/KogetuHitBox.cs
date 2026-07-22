using UnityEngine;

public class KogetuHitBox : MonoBehaviour
{
    [SerializeField] private Collider _hitBox;
    [SerializeField] private float _damage = 20f;

    private void Awake()
    {
        _hitBox.enabled = false;
    }

    public void EnableHitBox()
    {
        _hitBox.enabled = true;
    }

    public void DisableHitBox()
    {
        _hitBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy == null)
        {
            return;
        }

        enemy.TakeDamage(_damage);
        Debug.Log(_damage);
    }
}
