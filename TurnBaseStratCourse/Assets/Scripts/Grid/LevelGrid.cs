using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    // This Class is responsible for utilizing the GridSystem class to generate the playable level 
    public static LevelGrid Instance {get; private set;} // Public attribute to allow external classes to read from this class but not write to it (Singleton)

    [SerializeField, Tooltip("Reference to Degbug object for Grid Objects")]
    private Transform _gridDebugObjectPrefab;

    // Reference to the GridSystem this level will use
    private GridSystem _gridSystem;

    private void Awake() 
    {
        if(Instance != null) // If an instance of this script already exists before this one
        {
            Debug.LogError("Cannot have more than one LevelGrid instance." + transform + " - " + Instance);
            Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
        Instance = this;

        // Create a new GridSystem & cache it inside local member
        _gridSystem = new GridSystem(10, 10, 2f);

        // Access the Gridsytem and setup DebugObjects for each cell in the system using the cached prefab
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        // Takes in a unit and a GridObject & sets the unit to that location
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition); // Caches the GridObject at the given GridPosition
        gridObject.AddUnit(unit);                                        // Set the unit for this GridObject
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) 
    {
        // Takes in a GridPosition in the sytem & returns the unit in that position if one is present
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    } 

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unitToRemove) 
    {
        // Takes in a GridPosition within the system & clears that position of the unit in that cell
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unitToRemove);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition); // Lambda Expression

    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool isValidGridPosition(GridPosition gridPosition) => _gridSystem.isValidGridPosition(gridPosition);

    public int GetWidth() => _gridSystem.GetWidth();

    public int GetHeight() => _gridSystem.GetHeight();

    public bool isGridPositionOccupied(GridPosition gridPositon)
    {
        // Takes in a GridPosition and Returns true if a unit is currently occupying this GridPosition
        GridObject gridObject = _gridSystem.GetGridObject(gridPositon); // Get access to the GridObject within the GridPosiiton
        return gridObject.IsOccupied();
    }

    public void ChangeUnitGridPosition(Unit movingUnit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        // Takes in a Unit, the GridPosition it is currently leaving, & the GridPosition it is arriving to & changes the unit's interna; GridPosition
        RemoveUnitAtGridPosition(fromGridPosition, movingUnit);         // Clear the Unit from the starting GridPosition
        AddUnitAtGridPosition(toGridPosition, movingUnit); // Add the Unit to the new GridPosition
    }
}
