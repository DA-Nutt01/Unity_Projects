using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    // Base Class for all actions 

    protected Unit   _unit;               // Reference to the Unit Component of this Unit; Every action will need this reference
    protected bool   _isActive;           // Flag for if this action is currently allowed to run or not
    protected Action _onActionComplete; // A ref to the callback func when an action is complete

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
}
