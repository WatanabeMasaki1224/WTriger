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
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

        if(enemy != null)
        {
            projectile.SetTargetPosition(enemy.transform.position);
        }
    }

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
