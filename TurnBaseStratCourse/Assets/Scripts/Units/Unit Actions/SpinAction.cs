using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    private void Update()
    {
        if (!_isActive) return; // Skip all other code while this action is not active

        float spinDegrees = 360f * Time.deltaTime; // The number of degress the unit will spin
        transform.eulerAngles += new Vector3(0, spinDegrees, 0);

        _totalSpinAmount += spinDegrees;
        if (_totalSpinAmount > 360f) 
        {
            ActionComplete();    // Call our delegate from the base class
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete) // This method takes in a delegate as a parameter; When calling Spin, it now needs a function to store that it will call once it is complete
    {
        _totalSpinAmount = 0; 

        // For execution order purposes, make sure ActionStart() is called after initial setup of this action
        ActionStart(OnActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // Returns a list of  valid GridPositions that are within this unit's max move distance
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition(); // Cache the current GridPositon of this unit

        return new List<GridPosition>{unitGridPosition}; // Return a list of this unit's GridPosition, since it is the only valid GridPosition it can spin on
    }
}
