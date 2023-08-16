using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour
{
    // Attach to Unit World UI Canavas in Unit Prefab

    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private Unit            _unit; // This unit
    [SerializeField] private Image           _healthBarImage;
    [SerializeField] private HealthSystem    _healthSysem;



    private void Start()
    {
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
        _healthSysem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }
    private void UpdateActionPointsText()
    {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = _healthSysem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
