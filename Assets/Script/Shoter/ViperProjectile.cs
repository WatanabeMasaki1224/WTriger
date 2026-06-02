using UnityEngine;

public class ViperProjectile : ProjectileBase
{
    public enum ViperPattern
    {
        SideCurve,     // 左右カーブ
        UpperCurve,    // 上方向カーブ
        BackCurve   // 散弾→再突撃
    }

    [Header("バイパー")]
    [SerializeField] private ViperPattern _pattern;
    [SerializeField] private float _turnSpeed = 10f;
    [Header("カーブ")]
    [SerializeField] private float _curveDistance = 5f;
    [Header("散弾と再突撃")]
    [SerializeField] private float _scatterPower = 0.3f;　//散弾の広がり具合
    [SerializeField] private float _redirectTime = 1.5f;　//何秒後に再誘導するか
    [SerializeField] private float _searchRadius = 15f;　//再突撃じのサーチ範囲
    private bool _finishedCurve; // 敵がいた場所にたどり着いたか

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _curvePoint;

    private bool _reachedCurvePoint;
    private bool _hasTarget;
    private bool _finishedPosition;

    /// <summary>
    /// バイパーの軌道パターンを設定
    /// </summary>
    /// <param name="pattern"></param>
    public void SetPattern(ViperPattern pattern)
    {
        _pattern = pattern;
    }

    /// <summary>
    /// 目標地点を設定し軌道を初期化
    /// </summary>
    /// <param name="targetPos"></param>
    public void SetTargetPosition(Vector3 targetPos)
    {
        _targetPosition = targetPos;
        _hasTarget = true;

        switch (_pattern)
        {
            case ViperPattern.SideCurve:
                SetupSideCurve(); 
                break;

            case ViperPattern.UpperCurve: 
                SetupUpperCurve();
                break;

            case ViperPattern.BackCurve: 
                SetupBackCurve();
                break;
        }
    }

    protected override void Update()
    {
        if (!_canMove)
        {
            return;
        }

        //敵なし → 直進
        if (!_hasTarget)
        {
            transform.position +=_moveDirection * _speed * Time.deltaTime;
            return;
        }

        switch (_pattern)
        {
            case ViperPattern.SideCurve:
                UpdateCurve();
                break;

            case ViperPattern.UpperCurve:
                UpdateCurve();
                break;

            case ViperPattern.BackCurve:
                Debug.Log("再突撃");
                break;
        }

        transform.position += _moveDirection * _speed * Time.deltaTime;
    }

    /// <summary>
    /// 左右カーブ軌道の設定
    /// </summary>
    private void  SetupSideCurve()
    {
        _startPosition = transform.position;
        Vector3 direction = (_targetPosition - _startPosition).normalized;
        Vector3 basePoint = Vector3.Lerp(_startPosition,_targetPosition,0.8f);
        Vector3 side = Vector3.Cross(Vector3.up, direction);
        float randomSide = Random.Range(0, 2) == 0 ? -1f : 1f;
        _curvePoint = basePoint + side * randomSide * _curveDistance;
    }

    /// <summary>
    /// 左右と上カーブ軌道をフレームごとに更新
    /// </summary>
    private void UpdateCurve()
    {
        if (_finishedCurve)
        {
            return;
        }

        Vector3 dir = _moveDirection;

        if (!_reachedCurvePoint)
        {
            Vector3 toCurve =(_curvePoint - transform.position).normalized;
            dir = Vector3.Slerp(_moveDirection, toCurve, _turnSpeed * Time.deltaTime );

            if (Vector3.Distance( transform.position, _curvePoint) < 1f)
            {
                _reachedCurvePoint = true;
            }
        }
        else
        {
            Vector3 toTarget =(_targetPosition - transform.position).normalized;
            dir = Vector3.Slerp(_moveDirection, toTarget, _turnSpeed * Time.deltaTime);

            // ロックオン時の地点に到達したら誘導終了
            if (Vector3.Distance(transform.position, _targetPosition) < 1f)
            {
                _finishedCurve = true;
            }
        }

        _moveDirection = dir.normalized;
    }

    /// <summary>
    /// 上方向カーブ軌道の設定
    /// </summary>
    private void SetupUpperCurve()
    {
        _startPosition = transform.position;
        Vector3 basePoint = Vector3.Lerp(_startPosition, _targetPosition, 0.8f);
        _curvePoint = basePoint + Vector3.up * _curveDistance;
    }

    private void SetupBackCurve()
    {

    }
}
