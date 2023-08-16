using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor.EditorTools;
using UnityEngine.EventSystems;

public abstract class BaseAction : MonoBehaviour
{
    // Base Class for all actions; Does not sit on an object and is instead inherited from all other actions

    // Static events are great in the sense that a script that needs to know whenever a certain thing happens across 
    // potentially dozens of instances of a script, the subscriber does not have to subscribe to each and every instance
    // it wishes to listen to. The static event will cover all instances
    public static event EventHandler OnAnyActionStarted; 
    public static event EventHandler OnAnyActionCompleted; 

    protected Unit   _unit;             // Reference to the Unit Component of this Unit; Every action will need this reference
    protected bool   _isActive;         // Flag for if this action is currently allowed to run or not
    protected Action _onActionComplete; // A ref to the callback func when an action is complete
    [SerializeField, Tooltip(" The number of action points this action takes to execute; defaults at 1")] 
    protected int _actionPointCost = 1; 

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public abstract string GetActionName(); // Abstract methods don't have a body; they MUST be implemented by all child classes
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public  virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        // Takes in a GridPosition & returns true if is a valid GridPosition within the Unit's movement range

        List<GridPosition> validGridPositionList = GetValidActionGridPositionList(); // Get the list of valid GridPositions within the unit's move range
        return validGridPositionList.Contains(gridPosition);                         // Return true if the given GridPosition is within the list of valid GridPositions in move rage
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost()
    {
        // Returns the number of AP this costs to execute this action
        return _actionPointCost;
    }

    protected void ActionStart(Action OnActionComplete)
    {
        _isActive = true;
        _onActionComplete = OnActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {   
        // Returns the Unit this action belongs to
        return _unit;
    }
}
