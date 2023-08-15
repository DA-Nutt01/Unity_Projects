using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer;

    private void Start()
    {
        TurnSystem.Instance.OnRoundChange += TurnSystem_OnRoundChange;
    }
    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return; // Pass while Player's turn

        _timer -= Time.deltaTime;
        if (_timer <= 0f) TurnSystem.Instance.NextTurn(); // End the Enemy turn when timer reaches 0 
        
    }

    private void TurnSystem_OnRoundChange(object sender, EventArgs e) // Subscriber Method
    {
        _timer = 2f; // When a turn ends, reset the timer
    }
}
