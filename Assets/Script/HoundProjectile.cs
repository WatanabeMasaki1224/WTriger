using UnityEngine;
using UnityEngine.EventSystems;

public class HoundProjectile : ProjectileBase
{
    [Header("Hound")]
    [SerializeField] private float _turnSpeed = 3f;　//曲がる速さ
    [SerializeField] private float _forwardWeight = 0.7f; //どのくらい前進を残すか
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    protected override void Update()
    {
        if (!_canMove)
        {
            return;
        }
        Vector3 dir = _moveDirection;

        if (_target != null)
        {
            Vector3 toTarget =
                (_target.position - transform.position).normalized;

            // 少しだけ追尾方向へ寄せる
            dir = Vector3.Slerp(
                _moveDirection,
                toTarget,
                _turnSpeed * Time.deltaTime
            );
        }
        _moveDirection = dir;
        transform.forward = _moveDirection;

        transform.position +=
            _moveDirection * _speed * Time.deltaTime;
        Debug.Log(_target);
        Debug.Log(_canMove);
        Debug.Log(_moveDirection);
    }
}
