using UnityEngine;

public class GrassHopper : MonoBehaviour
{
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private float _dashTime = 0.25f;
    private PlayerController _player;

    void Start()
    {
        _player = GetComponent<PlayerController>();
    }

   
}
