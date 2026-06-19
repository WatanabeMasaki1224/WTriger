using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _trionGauge;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _killText;

    [Header("Player")]
    [SerializeField] private PlayerStatus _playerStatus;

    private float _gameTime;
    private int _killCount;

    void Update()
    {
        _gameTime += Time.deltaTime;

        UpdateUI();
    }

    private void UpdateUI()
    {
        _trionGauge.fillAmount =_playerStatus.CurrentTrion / _playerStatus.MaxTrion;

        _timerText.text =
            $"Time : {_gameTime:F1}";

        _killText.text =
            $"Kill : {_killCount}";
    }

    public void AddKillCount()
    {
        _killCount++;
    }
}
