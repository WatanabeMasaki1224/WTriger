using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Trion")]
    [SerializeField] private float _maxTrion = 100f;
    private float _currentTrion;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _damageSE;
    public float CurrentTrion => _currentTrion;
    public float MaxTrion => _maxTrion;

    private void Awake()
    {
        _currentTrion = _maxTrion;
    }

    /// <summary>
    /// トリオンを消費できたか判定する関数
    /// </summary>
    public bool ConsumeTrion(float amount)
    {
        if(_currentTrion < amount)
        {
            return false;
        }

        _currentTrion -= amount;
        return true;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public void TakeDamage(float damage)
    {
        _currentTrion -= damage;
        _audioSource.PlayOneShot(_damageSE);

        if (_currentTrion <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("トリオン切れ");
    }
}
