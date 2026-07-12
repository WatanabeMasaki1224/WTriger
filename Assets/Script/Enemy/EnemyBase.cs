using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _hp = 100f;
    private EnemySpawner _spawner;
    protected float _maxHP;
    [SerializeField] private GameObject _lockOnMarker;
    [SerializeField] private float _markerDistance = 15f;
    protected Transform _player;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSE;
    public float HP => _hp;
    public float MaxHP => _maxHP;

    protected virtual void Start()
    {
        _maxHP = _hp;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        UpdateLockOnMarker();
    }

    protected virtual void Move()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        _hp -= damage;
        _audioSource.PlayOneShot(_hitSE);
        StartCoroutine(HitStop());

        Debug.Log($"EnemyDmage:{damage} ");

        if (_hp <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitStop()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(0.05f);

        Time.timeScale = 1f;
    }

    protected virtual void Die()
    {
        if (_spawner != null)
        {
            _spawner.EnemyDead(this);
        }

        Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    protected virtual void UpdateLockOnMarker()
    {
        if (_player == null || _lockOnMarker == null)
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, _player.position);

        _lockOnMarker.SetActive(distance <= _markerDistance);
    }
}
