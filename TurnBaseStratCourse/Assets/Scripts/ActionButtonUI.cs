using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI _buttonText; // Ref to TMP text obj for this UI action button
    [SerializeField] private Button          _button;     // Ref to the button

    public void SetBaseAction(BaseAction baseAction)
    {
        // Takes in a unit action and assigns the fucntionality & text of this button to that action
        _buttonText.text = baseAction.GetActionName().ToUpper();
    }

}   
