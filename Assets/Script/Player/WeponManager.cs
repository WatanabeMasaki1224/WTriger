using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeponManager : MonoBehaviour
{
    [Header("Main Weapon")]
    [SerializeField] private MainWeponBase[] _mainWeapons;

    [Header("Sub Weapon")]
    [SerializeField] private SubWeponBase[] _subWeapons;

    private int _mainIndex;
    private int _subIndex;
    public static event Action OnMainWeponChanged;
    public MainWeponBase[] MainWeapons => _mainWeapons;
    public int MainIndex => _mainIndex;

    /// <summary>
    /// 現在選択中のメイン武器を使用する
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
    /// 現在選択中のサブ武器を使用する
    /// </summary>
    public void OnSubFire(InputAction.CallbackContext context)
    {
        if (_subWeapons.Length == 0)
        {
            return;
        }

        _subWeapons[_subIndex].OnFire(context);
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

        //Indexが設定数より上回った場合0に
        if (_mainIndex >= _mainWeapons.Length)
        {
            _mainIndex = 0;
        }

        // UIへ武器変更を通知
        OnMainWeponChanged?.Invoke();  
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

        //Indexが設定数より上回った場合0に
        if (_subIndex >= _subWeapons.Length)
        {
            _subIndex = 0;
        }

        Debug.Log($"サブ武器 : {_subWeapons[_subIndex].name}");
    }

    /// <summary>
    /// 現在のメイン武器が攻撃中かどうか
    /// </summary>
    public bool IsShooting
    {
        get
        {
            if(_mainWeapons.Length == 0)
                return false;
            return _mainWeapons[_mainIndex].IsShooting;
        }
    }
}
