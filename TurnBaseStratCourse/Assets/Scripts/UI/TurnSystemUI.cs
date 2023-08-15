using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button          _endTurnButton;               
    [SerializeField] private TextMeshProUGUI _roundCountText;      
    [SerializeField] private GameObject      _enemyTurnUIObject;


    private void Start()
    {
        UpdateRoundText();
        UpdateEnemyTurnUI();
        _endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn()); // Assigns the OnClick() event of this button to call NextTurn() when clicked

        TurnSystem.Instance.OnRoundChange += TurnSystem_OnRoundChange;            // Subscribe method to listen to event
    }

    private void UpdateRoundText()
    {
        // Updates the UI text with the current round
        _roundCountText.text = $"ROUND: {TurnSystem.Instance.GetRoundCount()}";
    }

    private void TurnSystem_OnRoundChange(object sender, EventArgs e)
    {
        UpdateRoundText(); // When the round changes, update the round text
        UpdateEnemyTurnUI();
    }

    private void UpdateEnemyTurnUI()
    {
        _enemyTurnUIObject.SetActive(!TurnSystem.Instance.IsPlayerTurn()); // Update visual for 'Enemy Turn' UI each turn change
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn()); // Update 'End Turn' button visual every turn change
    }
}
