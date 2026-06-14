using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SubWeponBase : MonoBehaviour
{
    /// <summary>
    /// サブトリガーを発動する
    /// </summary>
    public abstract void OnFire(InputAction.CallbackContext context);
}
