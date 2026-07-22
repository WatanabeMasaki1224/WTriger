using UnityEngine;
using UnityEngine.InputSystem;

public class MainWeponBase : MonoBehaviour
{
    protected bool _isShooting;
    public bool IsShooting => _isShooting;
    [SerializeField] private string _weaponName;
    public string WeaponName => _weaponName;

    public virtual void OnFire(InputAction.CallbackContext context)
    {
        
    }
}
