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
}
