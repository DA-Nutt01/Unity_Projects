using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Lumin;

public class UnitActionSystem : MonoBehaviour
{   
    // This Class handles selecting units and selecting actions for that unit to complete

    public static UnitActionSystem Instance {get; private set;} // Public attribute to allow external classes to read from this class but not write to it (Singleton)
    public event EventHandler OnSelectedUnitChange;             // Event for when selected unit changes
    public event EventHandler OnSelectedActionChange;           // Event for when selected action changes
    public event EventHandler<bool> OnBusyChange;               // Event for when the _isBusy flag changes; this event will take in _isBusy as a parameter
    public event EventHandler OnActionStarted;                  // Event for when an action starts, indicatiing when AP have been spent
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
        if (_isBusy) return;                                        // Skips the rest of Update if an action is currently being exectuted

        if (!TurnSystem.Instance.IsPlayerTurn()) return;            // Disable Player Unit Selection while it is the enemy's turn

        if (EventSystem.current.IsPointerOverGameObject())  return; // If the mouse is hovering over an UI Elements

        if (TryHandleUnitSelection())                               // When left clicking, if the mouse hits a unit, it selects that unit and makes sure not to call a unit action
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

            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))        // If the GridPosition is valid for the selected action
            {   
                if (_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) // IF Ap succesfully spent on this action
                {
                    SetBusy();
                    _selectedAction.TakeAction(mouseGridPosition, ClearBusy);        // Call the action

                    OnActionStarted?.Invoke(this, EventArgs.Empty);                  // Fire this event if there are any subscribers
                }
            }
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChange?.Invoke(this, _isBusy); // Fire the event if there any subscribers
    }

    private void ClearBusy()
    {
        _isBusy = false;
        OnBusyChange?.Invoke(this, _isBusy); // Fire the event if there are any subscribers
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            // Shoot a ray from the cam to mouse position
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _unitMask)) // If the ray hits a collider on the unit mask
            {
                if(hit.transform.TryGetComponent<Unit>(out Unit unit)) 
                {
                    if (unit == _selectedUnit) return false; // If the selected unit is already selected, return

                    if (unit.IsEnemy()) return false;        // The player cannot select Enemy Units

                    SelectUnit(unit);                        // Selects unit if it has a Unit Component
                }
                return true;                                 // Returns true if unnit was selected
            }     
        }         
        
        return false;                                        // Returns false if no unit was found
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

        OnSelectedActionChange?.Invoke(this, EventArgs.Empty); // Fire the event if there are any subscribers to it
    }

    public Unit GetSelectedUnit() // Create a public Get function to grant access to private fields to other classes
    {
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        // Returns the currently selected unit action
        return _selectedAction;
    }
}
