using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitManager : MonoBehaviour
{   
    // This Class handles selecting units and selecting actions for that unit to complete
    public static UnitManager Instance {get; private set;} // Public attribute to allow external classes to read from this class but not write to it (Singleton)

    public event EventHandler OnSelectedUnitChange; // Event

    [SerializeField, Tooltip("The currently selected unit the player is issuing commands to")]
    private Unit _selectedUnit;

    [SerializeField, Tooltip("Layer Mask to filter units when selecting units to command")]
    private LayerMask _unitMask;

    void Awake() 
    {
        if(Instance != null) // If an instance of this script already exists before this one
        {
            Debug.LogError("Cannot have more than one UnitManager instance." + transform + "-" + Instance);
            Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
        Instance = this;
    }
    void Update()
    { 
         if (Input.GetMouseButtonDown(0)) // left Click
         {
            if(TryHandleUnitSelection()) return; // Prevents unit from moving immediately when selecting a unit

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseController.GetMousePosition()); // Convert the mouse position into a GridPositon

            if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) // Check if the selected position is a valid GridPosition
            {
                if (_selectedUnit != null) _selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }
            else Debug.LogError("GridPosition out of range or invalid");
            
         }

         if (Input.GetMouseButtonDown(1)) // Right Click
         {
            if (_selectedUnit != null) _selectedUnit.GetSpinAction().Spin();
         }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                       // Shoot a ray from the cam to mouse position
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _unitMask))                            // If the ray hits a collider on the unit mask
        {
            if(hit.transform.TryGetComponent<Unit>(out Unit unitComponent)) SelectUnit(unitComponent);     // Selects unit if it has a Unit Component
            return true;                                                                                   // Returns true if unnit was selected
        }
        return false;                                                                                      // Returns false if no unit was found
    }

    private void SelectUnit(Unit unitToSelect)
    {
        _selectedUnit = unitToSelect;
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty); // Fire the event if there are any subscribers to it
        
    }

    public Unit GetSelectedUnit() // Create a public Get function to grant access to private fields to other classes
    {
        return _selectedUnit;
    }
}
