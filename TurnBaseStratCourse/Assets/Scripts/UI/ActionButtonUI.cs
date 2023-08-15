using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{   
    // This class must be attached to the Action Button UI Prefab, as it will be the underlying function for all unit action buttons

    [SerializeField] private TextMeshProUGUI _buttonText;     // Ref to TMP text obj for this UI action button
    [SerializeField] private Button          _button;         // Ref to the button
    [SerializeField] private GameObject      _selectedGFXObj; // Ref to 'Selected' Sprite asset

    private BaseAction                       _baseAction;     // A ref to whatever unit action is assigned to this button 

    public void SetBaseAction(BaseAction baseAction)
    {
        // Takes in a unit action and assigns the fucntionality & text of this button to that action
        _baseAction = baseAction; // Assign the Internal tracker for the unit action this button is assigned

        _buttonText.text = baseAction.GetActionName().ToUpper(); // Assign Text

        _button.onClick.AddListener(() => { // Anonymous function/Lambda expression definition
            // Assign Function
            UnitActionSystem.Instance.SetBaseAction(baseAction);
        }); 

    }

    public void UpdateSelectedVisual()
    {
        // Updates the selected GFX based on if this action is currently selected or not
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction(); // Cache a ref to the currently selected action
        _selectedGFXObj.SetActive(selectedBaseAction == _baseAction);                  // Set the selected GFX on if the selected action is this one

    }
}   
