using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object will be created for each node in the Grid System to hold that position
public class GridObject 
{
    // The grid position of this ibject within the Grid System
    private GridPosition _gridPosition;

    // The GridSystem that created this object
    private GridSystem   _gridSystem;

    [SerializeField, Tooltip("The Unit currently occupying this GridObject; READONLY")]
    private List<Unit> _occupyingUnitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition) // Public Constructor
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _occupyingUnitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in _occupyingUnitList)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n " + unitString;
    }

    public void AddUnit(Unit unit) 
    {
        // Takes in a Unit and sets it as the current one occupying this GridObject
        _occupyingUnitList.Add(unit);
    }

    public void RemoveUnit(Unit unitToRemove)
    {
        _occupyingUnitList.Remove(unitToRemove);
    }

    public List<Unit> GetUnitList()
    {
        // Returns the current unit occupying this GridObject
        return _occupyingUnitList;
    }

    public bool IsOccupied()
    {
        // Returns true if this GridObject currently has a Unit occupying it 
        return _occupyingUnitList.Count > 0;
    }
}
