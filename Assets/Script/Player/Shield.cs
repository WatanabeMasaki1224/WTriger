using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    public enum ShieldType
    {
        Front,
        Full
    }

    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private GameObject _frontShieldPrefab;
    [SerializeField] private GameObject _fullShieldPrefab;
    [SerializeField] private float _frontShieldCost = 5f;
    [SerializeField] private float _fullShieldCost = 10f;
    private bool _isShieldActive;
    private ShieldType _currentShieldType;
    private GameObject _currentShield;
    private float _timer;


    private void Update()
    {
        if(!_isShieldActive)
        {
            return;
        }

        _timer = Time.deltaTime;

        if(_timer < 1f)
        {
            return ;
        }

        _timer =0f;
        float cost = 0f;

        switch(_currentShieldType)
        {
            case ShieldType.Front:
                cost = _frontShieldCost;
                break;

            case ShieldType.Full:
                cost = _fullShieldCost;
                break;
        }

        if(!_playerStatus.ConsumeTrion(cost))
        {
            DisableShield();
        }
    }

    /// <summary>
    /// 前方シールド入力
    /// </summary>
    public void OnFrontShield(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ActivateShield(ShieldType.Front);
        }

        if (context.canceled &&
            _currentShieldType == ShieldType.Front)
        {
            DisableShield();
        }
    }

    /// <summary>
    /// 全方位シールド入力
    /// </summary>
    public void OnFullShield(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ActivateShield(ShieldType.Full);
        }

        if (context.canceled &&
            _currentShieldType == ShieldType.Full)
        {
            DisableShield();
        }
    }

    /// <summary>
    /// シールドを展開する
    /// </summary>
    private void ActivateShield(ShieldType shieldType)
    {
        DisableShield(); //すでにシールドがある場合削除するため
        GameObject shieldPrefab = null;

        switch (shieldType)
        {
            case ShieldType.Front:
                shieldPrefab = _frontShieldPrefab;
                break;

            case ShieldType.Full:
                shieldPrefab = _fullShieldPrefab;
                break;
        }

        _currentShield = Instantiate(shieldPrefab,transform.position,transform.rotation,transform);

        _currentShieldType = shieldType;
        _isShieldActive = true;
        _timer = 0f;
    }

    /// <summary>
    /// シールドを解除する
    /// </summary>
    private void DisableShield()
    {
        _isShieldActive = false;
        _timer = 0f;

        if (_currentShield != null)
        {
            Destroy(_currentShield);
            _currentShield = null;
        }
    }
}
