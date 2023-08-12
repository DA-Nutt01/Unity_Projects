using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonContainerPrefab; // Ref to the container for all UI action buttons
    [SerializeField] private Transform _actionButtonPrefab;          // Ref to the UI action button prefab to be instantiated
    private void Start()
    {
        UnitManager.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged; // Subscribe UnitActionSystem_OnSelectedUnitChanged to the event
        CreateUnitActionButton();
    }
   private void CreateUnitActionButton()
   {
        Unit selectedUnit = UnitManager.Instance.GetSelectedUnit();          // Cache the currentlt selected unit

        foreach (Transform buttonTransform in _actionButtonContainerPrefab)  // For each button currently in the action container
        {
            // Destroy that button (We need to clear the old selected unit's buttons before creating new ones)
            Destroy(buttonTransform.gameObject);
        }

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray()) // For each action on this unit
        {
            // Instantiate a UI button to represent that action as a child of the UI container
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerPrefab);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>(); // Cache a ref to the newly created button
            actionButtonUI.SetBaseAction(baseAction);                                             // Set the functionality this new button to the currently iterated unit action
        }
   }

   private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) // Subscribing method for OnSelectedUnitChanged
   {
        CreateUnitActionButton(); // When the event is triggered, recreate the buttons for the newly selected unit
   }


}
