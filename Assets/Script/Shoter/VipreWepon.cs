using UnityEngine;
using UnityEngine.InputSystem;

public class VipreWepon : ShooterWeponBase
{
    [Header("バイパー")]
    [SerializeField] private ViperProjectile _viperPrefab;
    [SerializeField] private ViperProjectile.ViperPattern _currentPattern;

    protected override void CreateProjectile(Vector3 spawnPosition)
    {
        ViperProjectile projectile = Instantiate(
            _viperPrefab,
            spawnPosition,
            Quaternion.LookRotation(_cameraTransform.forward)
        );

        projectile.SetPattern(_currentPattern);
        StartCoroutine(FireProjectile(projectile));
        Transform target = GetLockTarget();

        if (target != null)
        {
            projectile.SetTargetPosition(target.position);
        }
    }

    /// <summary>
    /// 左右カーブモードに変更
    /// </summary>
    /// <param name="context"></param>
    public void OnViper1(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _currentPattern =
            ViperProjectile.ViperPattern.SideCurve;

        Debug.Log("左右カーブに切り替え");
    }

    /// <summary>
    /// 上方向カーブモードに変更
    /// </summary>
    /// <param name="context"></param>
    public void OnViper2(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _currentPattern =
            ViperProjectile.ViperPattern.UpperCurve;

        Debug.Log("上カーブに切り替え");
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

        _currentPattern =
            ViperProjectile.ViperPattern.BackCurve;

        Debug.Log("再突撃に切り替え");
    }
}
