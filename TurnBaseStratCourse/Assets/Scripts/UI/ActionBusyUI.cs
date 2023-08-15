using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    // This Script sits on the parent ActionBusyUI Obj 

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange; // Subscribe a method to listen to the OnBusyChange event; Whenever the event triggers it will call the method(s) subscribed to it
        Hide(); 
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChange(object sender, bool isBusy)
    {
        if (isBusy) Show(); // If the ActionSystem is currently busy, Show the 'Busy' UI
        else Hide();        // Otherwise Hide it
    }
}
