using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;                // Ref to End Turn UI Button
    [SerializeField] private TextMeshProUGUI _roundCountText;      // Ref to UI text for round counter

    private void Start()
    {
        UpdateRoundText();
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
    }
}
