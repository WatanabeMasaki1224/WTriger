using Unity.VisualScripting;
using UnityEngine;

public class EnemyShield : EnemyBase
{
    [Header("Move")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _minDistance = 5f;  
    [SerializeField] private float _maxDistance = 10f;
    [Header("Shield")]
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Transform _shieldPoint;

    private Transform _player;
    private GameObject _shield;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        CreateShield();
    }

    protected override void Update()
    {
        if (_player == null)
        {
            return;
        }

        Move();
        LookPlayer();
    }


    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, _player.position);
        // ‰“‚·‚¬‚é‚Ì‚Å‹ß•t‚­
        if (distance > _maxDistance)
        {
            Vector3 dir = (_player.position - transform.position).normalized;
            transform.position += dir * _moveSpeed * Time.deltaTime;
        }
        // ‹ß‚·‚¬‚é‚Ì‚Å—£‚ê‚é
        else if (distance < _minDistance)
        {
            Vector3 dir = (transform.position - _player.position).normalized;
            transform.position += dir * _moveSpeed * Time.deltaTime;
        }
        // ˆê’è‹——£ŠÔ‚È‚ç‰½‚à‚µ‚È‚¢
    }


    private void LookPlayer()
    {
        Vector3 dir = _player.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);
    }


    private void CreateShield()
    {
        _shield = Instantiate(_shieldPrefab,_shieldPoint.position,transform.rotation,transform);
    }
}
