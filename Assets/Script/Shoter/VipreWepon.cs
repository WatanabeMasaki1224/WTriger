using UnityEngine;
using UnityEngine.InputSystem;

public class VipreWepon : ShooterWeponBase
{
    [Header("バイパー")]
    [SerializeField] private ViperProjectile _viperPrefab;
    [SerializeField] private ViperProjectile.ViperPattern _currentPattern;

    /// <summary>
    /// 弾を生成し、軌道パターンと目標地点を設定する
    /// </summary>
    /// <param name="spawnPosition"></param>
    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        ViperProjectile projectile = Instantiate(
            _viperPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );

        // 現在の軌道パターンを設定
        projectile.SetPattern(_currentPattern);
        // 発射処理を開始
        StartCoroutine(FireProjectile(projectile));
        Transform target = GetLockTarget();

        // ロックオン対象がいる場合は目標地点を設定
        if (target != null)
        {
            projectile.SetTargetPosition(target.position);
        }
    }

    /// <summary>
    /// 左右カーブモードに切り替え
    /// </summary>
    /// <param name="context"></param>
    public void OnViper1(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _currentPattern =　ViperProjectile.ViperPattern.SideCurve;
    }

    /// <summary>
    /// 上方向カーブモードに切り替え
    /// </summary>
    /// <param name="context"></param>
    public void OnViper2(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _currentPattern =　ViperProjectile.ViperPattern.UpperCurve;
    }

    /// <summary>
    /// 再突撃モードに変更
    /// </summary>
    /// <param name="context"></param>
    public void OnViper3(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _currentPattern = ViperProjectile.ViperPattern.BackCurve;

        Debug.Log("再突撃に切り替え");
    }
}
