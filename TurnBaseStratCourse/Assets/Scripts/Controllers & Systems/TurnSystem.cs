using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance {get; private set;}     // Public attribute to allow external classes to read from this class but not write to it (Singleton)

    public event EventHandler OnRoundChange;                  // Event for when the current round changes
    private int                         _roundCount = 1;      // The total number of turns the player has taken; Always starts on Round 1 obviously
    [SerializeField] private bool       _isPlayerTurn = true; // Flag for when it is currently the player's turn or not; defaults to true

    void Awake() 
    {
        if(Instance != null) // If an instance of this script already exists before this one
        {
            Debug.LogError("Cannot have more than one TurnSystem instance." + transform + "-" + Instance);
            Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
        Instance = this;
    }
    public void NextTurn()
    {
        _roundCount ++;                               // Increment the turn count by 1
        _isPlayerTurn = !_isPlayerTurn;               // Invert this bool
        OnRoundChange?.Invoke(this, EventArgs.Empty); // Fire this event if there are any subscribers
    }

    public int GetRoundCount()
    {
        // Returns the current round count
        return _roundCount;
    }

    public bool IsPlayerTurn()
    {
        return _isPlayerTurn;
    }
}
