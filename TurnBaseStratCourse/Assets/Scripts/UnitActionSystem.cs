using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class UnitActionSystem : MonoBehaviour
{   
    // This Class handles selecting units and selecting actions for that unit to complete
    public static UnitActionSystem Instance {get; private set;} // Public attribute to allow external classes to read from this class but not write to it (Singleton)
    public event EventHandler OnSelectedUnitChange;             // Event
    private bool _isBusy;                                       // Flag to determine if any action is currently being executed; Only one action can be active at a time ever
    private BaseAction _selectedAction;                         // The currently selected action for a unit to take

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

    private void Start()
    {
        SelectUnit(_selectedUnit); // The Game will start with a unit already selected, we call this to make sure an action is also selected for it
    }
    void Update()
    { 
        if (_isBusy) return; // Skips the rest of Update if an action is currently being exectuted

        if (TryHandleUnitSelection()) // When left clicking, if the mouse hits a unit, it selects that unit and makes sure not to call a unit action
        {
            return;
        }
        HandleSelectedAction();
        
    } 

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButton(0)) // Left Click
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseController.GetMousePosition()); // Convert the mouse position into a GridPositon

            switch (_selectedAction)
            {
                case MoveAction moveAction:
                    if (moveAction.IsValidActionGridPosition(mouseGridPosition, ClearBusy)) // Check if the selected position is a valid GridPosition
                    {
                        SetBusy();
                        moveAction.Move(mouseGridPosition);
                    }
                    else Debug.LogError("GridPosition out of range or invalid");
                    break;
                case SpinAction spinAction:
                    SetBusy();
                    spinAction.Spin(ClearBusy);
                    break;
            }
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
    }

    private void ClearBusy()
    {
        _isBusy = false;
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                       // Shoot a ray from the cam to mouse position
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _unitMask))                            // If the ray hits a collider on the unit mask
            {
                if(hit.transform.TryGetComponent<Unit>(out Unit unitComponent)) SelectUnit(unitComponent);     // Selects unit if it has a Unit Component
                return true;                                                                                   // Returns true if unnit was selected
            }     
        }         
        
        return false;                                                                        // Returns false if no unit was found
    }

    private void SelectUnit(Unit unitToSelect)
    {
        _selectedUnit = unitToSelect;                        // Set the unitToSelect as the new selected Unit
        SetBaseAction(unitToSelect.GetMoveAction());         // By default, select the Move Action when selecting a new unit
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty); // Fire the event if there are any subscribers to it
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        // Sets the currently selected action for the currently selected unit
        _selectedAction = baseAction;
    }

    public Unit GetSelectedUnit() // Create a public Get function to grant access to private fields to other classes
    {
        return _selectedUnit;
    }
}
