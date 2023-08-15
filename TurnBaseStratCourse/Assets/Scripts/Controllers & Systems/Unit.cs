using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Tooltip("READONLY: The current GridPosition this unit is occupying")]
    private GridPosition _currentGridPosition;

    private BaseAction[] _baseActionArray;  // An array to hold all unit actions on this unit
    private MoveAction   _moveAction;       // Reference to MoveAction script component
    private SpinAction   _spinAction;       // Reference to MoveAction script component
    private int          _actionPoints = 2; // The number of actions this unit can take per turn

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>(); // Takes all scripts that are children of BaseAction on this unit and stores them in the array
    }
    void Start()
    {
        _currentGridPosition =  LevelGrid.Instance.GetGridPosition(transform.position); // Cache this unit's starting GridPosition
        LevelGrid.Instance.AddUnitAtGridPosition(_currentGridPosition, this);           // Set the current position of this unit in it's current GridPosition
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);                 // Recalculate the GridPosition of this unit each frame
        if (_currentGridPosition != newGridPosition)                                                           // If this unit changed GridPosition
        {
            // This unit has changed its GridPosition
            LevelGrid.Instance.ChangeUnitGridPosition(this, _currentGridPosition, newGridPosition);            // Change the current GridPosition of this Unit
            _currentGridPosition = newGridPosition;                                                            // Update the Unit's current GridPosition
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
        return (_actionPoints >= baseAction.GetActionPointCost());
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
    }

    public int GetActionPoints()
    {
        // Returns the current total Action Points this unit has this turn
        return _actionPoints;
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
}
