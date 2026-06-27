using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController _characterController;
    private Animator _animator;
    Vector2 _moveInput;
    Vector2 _lookInput;
    float _verticalVelocity;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _jumpPower = 8f;
    [SerializeField] float _gravity = -9.8f;
    [SerializeField] Transform _cameraTransform;
    float _cameraPitch = 0f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        //マウスカーソルを画面中に固定
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Look();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _characterController.isGrounded)
        {
            _verticalVelocity = _jumpPower;
            _animator.SetTrigger("Jump");
            _animator.SetBool("Grounded",_characterController.isGrounded);
        }
    }

    void Move()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 move =forward * _moveInput.y + right * _moveInput.x;
        _characterController.Move(move * _moveSpeed * Time.deltaTime);

        if(_characterController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;
        _characterController.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
        float speed = _moveInput.magnitude;
        _animator.SetFloat("Speed", speed);
        _animator.SetFloat("MoveX", _moveInput.x);
        _animator.SetFloat("MoveY", _moveInput.y);
    }

    void Look()
    {
        float mouseX = _lookInput.x * 0.1f;
        float mouseY = _lookInput.y * 0.1f;

        transform.Rotate(0, mouseX, 0);

        _cameraPitch -= mouseY;

        _cameraPitch = Mathf.Clamp(_cameraPitch, -80f, 80f);

        _cameraTransform.localRotation =
            Quaternion.Euler(_cameraPitch, 0, 0);
    }
}
