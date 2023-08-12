using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{   
    // This class must be attached to the Action Button UI Prefab, as it will be the underlying function for all unit action buttons

    [SerializeField] private TextMeshProUGUI _buttonText; // Ref to TMP text obj for this UI action button
    [SerializeField] private Button          _button;     // Ref to the button

    public void SetBaseAction(BaseAction baseAction)
    {
        // Takes in a unit action and assigns the fucntionality & text of this button to that action
        _buttonText.text = baseAction.GetActionName().ToUpper(); // Assign Text

        _button.onClick.AddListener(() => { // Anonymous function/Lambda expression definition
            // Assign Function
            UnitActionSystem.Instance.SetBaseAction(baseAction);
        }); 

    }
}   
