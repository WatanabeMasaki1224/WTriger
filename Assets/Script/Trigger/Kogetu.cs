using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class Kogetu : MainWeponBase
{
    [SerializeField] private float _attackTime = 0.2f;
    [SerializeField] private float _coolTime = 0.5f;
    [SerializeField] private KogetuHitBox _hitBox;

    public override void OnFire(InputAction.CallbackContext context)
    {
       if(!context.performed)
        {
            return;
        }
       if(IsShooting)
        {
            return ;
        }

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isShooting = true;
        // 攻撃判定ON
        _hitBox.EnableHitBox();

        yield return new WaitForSeconds(_attackTime);

        // 攻撃判定OFF
        _hitBox.DisableHitBox();

        // クールタイム
        yield return new WaitForSeconds(_coolTime);

        _isShooting = false;
    }

}
