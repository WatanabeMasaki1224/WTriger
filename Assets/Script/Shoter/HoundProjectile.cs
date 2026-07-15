using UnityEngine;
using UnityEngine.EventSystems;

public class HoundProjectile : ProjectileBase
{
    [Header("Hound")]
    [SerializeField] private float _turnSpeed = 15f;　//曲がる速さ
    [SerializeField] private float _forwardWeight = 0.7f; //どのくらい前進を残すか
    private Transform _target;
    private Vector3 _startPosition;
    private Vector3 _curvePoint;
    private bool _reachedCurvePoint;
    [SerializeField] private float _curveDistance = 5f;　//どれくらい膨らむか
    private bool _hasTarget;

    /// <summary>
    /// 追尾対象を設定し軌道を初期化する
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        _target = target;
        _startPosition = transform.position;
        _reachedCurvePoint = false;
        _hasTarget = target != null;
        //ロックオン対象だいないときは直線
        if (!_hasTarget) return;
        Vector3 targetPos = _target.position;
        //敵との中間地点を計算
        Vector3 midPoint = (_startPosition + targetPos) * 0.5f;
        //カーブ用の横方向ベクトルを作成
        Vector3 side =Vector3.Cross(Vector3.up,(target.position - _startPosition).normalized);
        _curvePoint = midPoint+ side * _curveDistance;
        
    }

    /// <summary>
    /// 弾の移動処理
    /// </summary>
    protected override void Update()
    {
        if (!_canMove)
        {
            return;
        }
        Vector3 dir = _moveDirection;

        //ターゲットなしなら直進だけ
        if (!_hasTarget)
        {
            _moveDirection = dir;
            transform.forward = _moveDirection;
            transform.position += _moveDirection * _speed * Time.deltaTime;
            return;
        }

        // カーブ処理
        if (!_reachedCurvePoint)
        {
            Vector3 toCuver = (_curvePoint - transform.position).normalized;
            dir = Vector3.Slerp(_moveDirection,toCuver, _turnSpeed * Time.deltaTime);
            if (Vector3.Distance( transform.position, _curvePoint) < 1f)
            {
                _reachedCurvePoint = true;
            }
        }

        // カーブ後はターゲットを追尾
        else if (_target != null)
        {
            Vector3 toTarget =
                (_target.position - transform.position).normalized;

            // 徐々にターゲット方向へ向きを変える
            dir = Vector3.Slerp( _moveDirection, toTarget,_turnSpeed * Time.deltaTime);
        }

        // 前進方向を残しつつ移動方向を更新
        _moveDirection = (_moveDirection * _forwardWeight + dir).normalized;
        transform.forward = _moveDirection;
        transform.position += _moveDirection * _speed * Time.deltaTime;
    }
}
