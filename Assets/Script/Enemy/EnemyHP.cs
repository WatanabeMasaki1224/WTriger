using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private EnemyBase _enemy;
    [SerializeField] private Color _normalColor = Color.green;
    [SerializeField] private Color _phase2Color = Color.yellow;
    [SerializeField] private Color _lastColor = Color.red;

    private void Update()
    {
        float hpRate = _enemy.HP / _enemy.MaxHP;
        _hpBar.fillAmount = hpRate;
        UpdateColor(hpRate);
    }

    void UpdateColor(float rate)
    {
        if (rate <= 0.25f)
        {
            _hpBar.color = _lastColor;
        }
        else if (rate <= 0.5f)
        {
            _hpBar.color = _phase2Color;
        }
        else
        {
            _hpBar.color = _normalColor;
        }
    }
}
