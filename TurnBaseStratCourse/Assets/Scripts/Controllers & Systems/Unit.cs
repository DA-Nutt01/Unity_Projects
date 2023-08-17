using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // An instance of this script sits on each and every unit

    public static event EventHandler OnAnyActionPointChange; // Event for when the action point changes on any unit

    [SerializeField, Tooltip("READONLY: The current GridPosition this unit is occupying")]
    private GridPosition _currentGridPosition; 
    private BaseAction[] _baseActionArray;  // An array to hold all unit actions on this unit
    private MoveAction   _moveAction;       // Reference to MoveAction script component
    private SpinAction   _spinAction;       // Reference to MoveAction script component
    private HealthSystem _healthSystem;
    [SerializeField, Tooltip("The max action points this unit has per round")] 
    private int _maxActionPoints = 2;
    [SerializeField] private int _currentActionPoints;       // The number of actions this unit can take per turn
    [SerializeField] private bool _isEnemy; // Flag for defining enemies

    private void Awake()
    {
        _currentActionPoints = _maxActionPoints;
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _healthSystem = GetComponent<HealthSystem>();
        _baseActionArray = GetComponents<BaseAction>(); // Takes all scripts that are children of BaseAction on this unit and stores them in the array
    }
    void Start()
    {
        _currentGridPosition =  LevelGrid.Instance.GetGridPosition(transform.position); // Cache this unit's starting GridPosition
        LevelGrid.Instance.AddUnitAtGridPosition(_currentGridPosition, this);           // Set the current position of this unit in it's current GridPosition

        TurnSystem.Instance.OnRoundChange += TurnSystem_OnRoundChange;                  // Subscribe to event to update this unit's action points at the start of the round
        _healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);                 // Recalculate the GridPosition of this unit each frame
        if (_currentGridPosition != newGridPosition)                                                           // If this unit changed GridPosition
        {
            // This unit has changed its GridPosition
            GridPosition oldGridPosition = _currentGridPosition;
            _currentGridPosition = newGridPosition;

            _currentGridPosition = newGridPosition;                                                            // Update the Unit's current GridPosition
            LevelGrid.Instance.ChangeUnitGridPosition(this, oldGridPosition, newGridPosition);            // Change the current GridPosition of this Unit

        }
    }

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return _currentGridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        // Returns the array of all unit actions for this unit
        return _baseActionArray;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        // Takes in an action and returns true this unit has enough AP to spend on that action; otherwise returns false
        return (_currentActionPoints >= baseAction.GetActionPointCost());
    }

    private void SpendActionPoints(int amount)
    {
        _currentActionPoints -= amount;

        OnAnyActionPointChange?.Invoke(this, EventArgs.Empty); // Fire this event if there are any subscribers
    }

    public int GetActionPoints()
    {
        // Returns the current total Action Points this unit has this turn
        return _currentActionPoints;
    }

    public bool IsEnemy()
    {
        return _isEnemy;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        // Takes in an action and attempts to spend the corresponding action points on that action
        // Returns true if successfully spends action points on the action; returns false otherwise

        if (CanSpendActionPointsToTakeAction(baseAction)) 
        {
            SpendActionPoints(baseAction.GetActionPointCost()); // Spend action points on given action
            return true;
        } 
        else 
        {
            Debug.LogError($"Not enough Action Points to take {baseAction}");
            return false; // Otherwise do not spend action points and return false
        }
    }

    private void TurnSystem_OnRoundChange(object sender, EventArgs e) // Subscriber Method; 'object sender' reps the obj that fired the event
    {
        if ((!_isEnemy && TurnSystem.Instance.IsPlayerTurn()) || (_isEnemy && !TurnSystem.Instance.IsPlayerTurn()))
        {
            _currentActionPoints = _maxActionPoints; // When a new rounds starts, reset the Action Points of this unit only if it is on the side that is currently its turn
            OnAnyActionPointChange?.Invoke(this, EventArgs.Empty); // Fire event
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        // Remove this unit from the GridSystem on death
        LevelGrid.Instance.RemoveUnitAtGridPosition(_currentGridPosition, this);
        Destroy(gameObject);
    }

    public void TakeDamage(int dmgAmount)
    {
        _healthSystem.TakeDamage(dmgAmount);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
