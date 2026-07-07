using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private EnemyBase _enemy;

    private void Update()
    {
        _hpBar.fillAmount = _enemy.HP / _enemy.MaxHP;
    }
}
