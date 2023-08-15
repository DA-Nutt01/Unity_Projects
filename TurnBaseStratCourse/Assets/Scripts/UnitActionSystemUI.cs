using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonContainerPrefab; // Ref to the container for all UI action buttons
    [SerializeField] private Transform _actionButtonPrefab;          // Ref to the UI action button prefab to be instantiated

    private List<ActionButtonUI>       _actionButtonUIList;          // A list of all currently active action buttons

    private void Awake()
    {
        _actionButtonUIList = new List<ActionButtonUI>();            // Instantiate the list 
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;   // Subscribe UnitActionSystem_OnSelectedUnitChanged to the event
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChanged; // Subscribe UnitActionSystem_OnSelectedUnitChanged to the event
        CreateUnitActionButton();
        UpdateSelectedVisual();
    }
   private void CreateUnitActionButton()
   {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();    // Cache the currentlt selected unit

        foreach (Transform buttonTransform in _actionButtonContainerPrefab) // For each button currently in the action container
        {
            
            Destroy(buttonTransform.gameObject);                            // Destroy that button (We need to clear the old selected unit's buttons before creating new ones)
        }

        _actionButtonUIList.Clear();                                        // Clear the list of all currently active buttons after deleting them

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray()) // For each action on this unit
        {
            
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerPrefab); // Instantiate a UI button to represent that action as a child of the UI container
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();             // Cache a ref to the newly created button
            actionButtonUI.SetBaseAction(baseAction);                                                         // Set the functionality this new button to the currently iterated unit action
            _actionButtonUIList.Add(actionButtonUI);                                                          // Add The ActionButtonUI component of this newly created button to the list of currently active buttons 
        }
   }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) // Subscribing method for OnSelectedUnitChanged
    {
        CreateUnitActionButton(); // When the event is triggered, recreate the buttons for the newly selected unit
        UpdateSelectedVisual();   // Update the visuals for valid GridPositions for the new action of the newly selected unit
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) // Subscribing method for OnSelectedUnitChanged
    {
        UpdateSelectedVisual(); // When the event is triggered, Update the visual for valid GridPositions for the newly selected action
    }


    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in _actionButtonUIList) // Loop throgh each ActionButtonUI in the interanl list
        {
            actionButtonUI.UpdateSelectedVisual();                     // Update the 'Selected' GFX for this button
        }
    }
}
