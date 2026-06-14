using UnityEngine;
using UnityEngine.InputSystem;

public class WeponManager : MonoBehaviour
{
    [Header("Main Weapon")]
    [SerializeField] private ShooterWeponBase[] _mainWeapons;

    [Header("Sub Weapon")]
    [SerializeField] private SubWeponBase[] _subWeapons;

    private int _mainIndex;
    private int _subIndex;

    /// <summary>
    /// 現在選択中のメイン武器を発射する
    /// </summary>
    public void OnMainFire(InputAction.CallbackContext context)
    {
        if (_mainWeapons.Length == 0)
        {
            return;
        }

        _mainWeapons[_mainIndex].OnFire(context);
    }

    /// <summary>
    /// 現在選択中のサブ武器を発動する
    /// </summary>
    public void OnSubFire(InputAction.CallbackContext context)
    {
        if (_subWeapons.Length == 0)
        {
            return;
        }

        _subWeapons[_mainIndex].OnFire(context);
    }

    /// <summary>
    /// メイン武器を切り替える
    /// </summary>
    public void OnNextMainWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _mainIndex++;

        if (_mainIndex >= _mainWeapons.Length)
        {
            _mainIndex = 0;
        }

        Debug.Log($"メイン武器 : {_mainWeapons[_mainIndex].name}");
    }

    /// <summary>
    /// サブ武器を切り替える
    /// </summary>
    public void OnNextSubWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _subIndex++;

        if (_subIndex >= _subWeapons.Length)
        {
            _subIndex = 0;
        }

        Debug.Log($"サブ武器 : {_subWeapons[_subIndex].name}");
    }
}
