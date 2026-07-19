using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GrassHopper : SubWeponBase
{
    [Header("Grasshopper")]
    [SerializeField] private float _jumpSpeed = 12f;
    [SerializeField] private float _coolTime = 1.5f;
    [SerializeField] private float _trionCost = 10f;

    private float _coolTimer;

    private PlayerController _player;
    private PlayerStatus _playerStatus;


    private void Start()
    {
        _player = GetComponent<PlayerController>();
        _playerStatus = GetComponent<PlayerStatus>();
    }


    private void Update()
    {
        if (_coolTimer > 0)
        {
            _coolTimer -= Time.deltaTime;
        }
    }


    /// <summary>
    /// グラスホッパー発動
    /// </summary>
    public override void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        // クールタイム中
        if (_coolTimer > 0)
        {
            return;
        }


        // トリオン消費
        if (!_playerStatus.ConsumeTrion(_trionCost))
        {
            return;
        }


        // 加速を追加
        _player.StartGrasshopper(_jumpSpeed);


        // クール開始
        _coolTimer = _coolTime;
    }
}