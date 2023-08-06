using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField, Tooltip("The currently selected unit the player is issuing commands to")]
    private Unit _selectedUnit;

    [SerializeField, Tooltip("Layer Mask to filter units when selecting units to command")]
    private LayerMask _unitMask;

    void Update()
    { 
         if (Input.GetMouseButtonDown(0)) 
        {
            if(TryHandleUnitSelection()) return; // Prevents unit from moving immediately when selecting a unit
            if(_selectedUnit != null) _selectedUnit.Move(MouseController.GetMousePosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                       // Shoot a ray from the cam to mouse position
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _unitMask))                            // If the ray hits a collider on the unit mask
        {
            if(hit.transform.TryGetComponent<Unit>(out Unit unitComponent)) _selectedUnit = unitComponent; // Selects unit if it has a Unit Component
            return true;                                                                                   // Returns true if unnit was selected
        }
        return false;                                                                                      // Returns false if no unit was found
    }
}
