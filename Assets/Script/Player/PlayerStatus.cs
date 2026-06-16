using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Trion")]
    [SerializeField] private float _maxTrion = 100f;
    private float _currentTrion;

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
            Debug.Log("トリオン不足");
            return false;
        }

        _currentTrion -= amount;
        Debug.Log($"残りトリオン：{_currentTrion}");
        return true;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public void TakeDamage(float damage)
    {
        _currentTrion -= damage;

        Debug.Log($"被弾 トリオン残量：{_currentTrion}");

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
