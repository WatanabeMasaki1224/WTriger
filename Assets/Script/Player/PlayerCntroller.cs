using Unity.VisualScripting;
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
    [SerializeField] float _mouseSensitivity = 0.05f;
    [SerializeField] Transform _cameraTransform;
    float _cameraPitch = 0f;
    private ShooterWeponBase _shooter;
    private WeponManager _weponManager;
    public CharacterController Controller => _characterController;
    public Transform CameraTransform => _cameraTransform;
    public Vector2 MoveInput => _moveInput;

    public bool CanMove { get; set; } = true;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _shooter = GetComponent<ShooterWeponBase>();
        //マウスカーソルを画面中に固定
        Cursor.lockState = CursorLockMode.Locked;
        _weponManager = GetComponent<WeponManager>();
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
        if (_shooter.IsShooting)
        {
            return;
        }

        if (context.performed && _characterController.isGrounded)
        {
            _verticalVelocity = _jumpPower;
            _animator.SetTrigger("Jump");
            _animator.SetBool("Grounded",_characterController.isGrounded);
        }
    }

    void Move()
    {
        if (!CanMove)
        {
            return;
        }

        // 攻撃中は移動できない
        if (_weponManager.IsShooting)
        {
            return;
        }

        // カメラの向きを基準に移動方向を計算
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

        // 重力を適用
        _verticalVelocity += _gravity * Time.deltaTime;
        _characterController.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
        // アニメーションへ移動情報を渡す
        float speed = _moveInput.magnitude;
        _animator.SetFloat("Speed", speed);
        _animator.SetFloat("MoveX", _moveInput.x);
        _animator.SetFloat("MoveY", _moveInput.y);
    }

    /// <summary>
    /// カメラの視点を操作する
    /// </summary>
    void Look()
    {
        float mouseX = _lookInput.x * _mouseSensitivity;
        float mouseY = _lookInput.y * _mouseSensitivity;
        // プレイヤーを左右に回転
        transform.Rotate(0, mouseX, 0);
        // カメラを上下に回転
        _cameraPitch -= mouseY;
        // カメラの上下角度を制限
        _cameraPitch = Mathf.Clamp(_cameraPitch, 0f, 50f);

        _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);
    }
}
