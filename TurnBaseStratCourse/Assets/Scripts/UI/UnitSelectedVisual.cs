using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField, Tooltip("The Unit component of this unit")]
    private Unit _unit; 

    [SerializeField, Tooltip("The Meshrenderer component of this object")]
    private MeshRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    { 
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitManager_OnSelectedUnitChange;
        UpdateVisual();
    }

    private void UnitManager_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if(UnitActionSystem.Instance.GetSelectedUnit() == _unit) // The selected unit is currently this unit
        {
            _renderer.enabled = true;                       // Turn on Selected Visual
        } else _renderer.enabled = false;                   // Otherwise turn off Selected Visual
    }

    private void OnDestroy() 
    {
        // Must unsubscribe from events that will call on null references once a unit is destroyed
        UnitActionSystem.Instance.OnSelectedUnitChange -= UnitManager_OnSelectedUnitChange;
    }
}
