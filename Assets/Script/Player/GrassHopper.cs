using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GrassHopper : SubWeponBase
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _time = 0.25f;
    private PlayerController _player;

    void Start()
    {
        _player = GetComponent<PlayerController>();
    }

    public override void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        _player.CanMove = false;

        Vector3 forward = _player.CameraTransform.forward;
        Vector3 right = _player.CameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector2 input = _player.MoveInput;

        Vector3 dir = forward * input.y + right * input.x;

        if (dir.sqrMagnitude < 0.01f)
        {
            dir = _player.transform.forward;
        }

        float timer = 0f;

        while (timer < _time)
        {
            _player.Controller.Move(dir.normalized * _speed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        _player.CanMove = true;
    }
}