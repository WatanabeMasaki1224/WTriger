using TMPro;
using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    [SerializeField] private WeponManager _weaponManager;
    [SerializeField] private TMP_Text[] _weaponTexts;

    private void OnEnable()
    {
        WeponManager.OnMainWeponChanged += UpdateUI;
    }

    private void OnDisable()
    {
        WeponManager.OnMainWeponChanged -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// •Ší‘I‘ğUI‚ğXV
    /// </summary>
    private void UpdateUI()
    {
        var weapons = _weaponManager.MainWeapons;
        int index = _weaponManager.MainIndex;

        for (int i = 0; i < _weaponTexts.Length; i++)
        {
            if (i == index)
            {
                _weaponTexts[i].text = " œ " + weapons[i].WeaponName;
            }
            else
            {
                _weaponTexts[i].text = " ü " + weapons[i].WeaponName;
            }
        }
    }
}
