using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Attach this to the EnemyAI game object; This script is the equivalent of the UnitActionSystem, only it 
    // works the enemy units;
    // The Architexture of the AI here is to create a loop of the AI taking turns with Enemy units untill all
    // action points have been spent. This is achieved by having SetStateTakingTurn() switch the state to 
    // TakingTurn, which first starts a delay timer; when timer hits 0, the TakeEnemyAction() method is called
    // to take a turn with one of the enemy units, with a callback to SetStateTakingTurn() to seal the loop.
    private enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy,
    }

    [SerializeField] 
    private State _currentState;
    private float _timer;

    private void Awake()
    {
        _currentState = State.WaitingForTurn;
    }
    private void Start()
    {
        TurnSystem.Instance.OnRoundChange += TurnSystem_OnRoundChange;
    }
    private void Update()
    {
        // Do nothing if it is the Player's turn
        if (TurnSystem.Instance.IsPlayerTurn()) return; 

        switch (_currentState)
        {
            case State.WaitingForTurn:
                break;
            case State.TakingTurn:
                // Decrement the timer over time
                _timer -= Time.deltaTime;
                // End the Enemy turn when timer reaches 0 
                if (_timer <= 0f) 
                {
                    // While enemy units are able to take actions, keep the state of this as Busy
                    if (TryTakeEnemyAIAction(SetStateTakingTurn)) _currentState = State.Busy;
                    else TurnSystem.Instance.NextTurn(); // When no enemy units are able to take an action, end the turn
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void TurnSystem_OnRoundChange(object sender, EventArgs e) // Subscriber Method
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            // When a turn ends and it was the Player's, start timer and set AI state
            _currentState = State.TakingTurn;
            _timer = 2f;
        }
    }

    private void SetStateTakingTurn()
    {   
        // Sets the _currentState to TakingTurn

        // Increment the timer a little to prevent enemy actions from triggering instantly
        _timer = 0.5f;
        _currentState = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAICompleteAction)
    {
        // This version of the method works for the entire enemy team
        // Returns true when a enemyUnit successfully takes an action
        // Returns false when a unit cannot take an action

        // Loop through every Enemy Unit and keeps taking actions with them until none are able to take actions
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            // Return true if this unit successfully took an action
           if(TryTakeEnemyAIAction(enemyUnit, onEnemyAICompleteAction)) return true;
        }
        // Return false if the unit was unable to take an action 
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAICompleteAction)
    {
        // This version of the method works for a single enemy unit
        // Returns true if all checks are passed and action is successfully taken
        // Returns false if the unit was unable to take an action

        SpinAction spinAction = enemyUnit.GetSpinAction();

         // Convert the mouse position into a GridPositon & cache it
        GridPosition actionGridPosition = enemyUnit.GetGridPosition(); 

        // If the GridPosition is NOT valid for the selected action
        if (!spinAction.IsValidActionGridPosition(actionGridPosition)) return false;       
               
        // IF Ap UNsuccesfully spent on this action
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;

        // Once the action clears all checks, actually execute the action & fire events 
        spinAction.TakeAction(actionGridPosition, onEnemyAICompleteAction);

        return true;
        
    }
}
