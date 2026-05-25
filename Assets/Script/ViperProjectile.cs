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

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _curvePoint;

    private bool _reachedCurvePoint;
    private bool _hasRedirected;

    private float _timer;

    public void SetPattern(ViperPattern pattern)
    {
        _pattern = pattern;
    }

    

}
