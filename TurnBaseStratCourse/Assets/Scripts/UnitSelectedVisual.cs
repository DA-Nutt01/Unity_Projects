using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
        UnitManager.Instance.OnSelectedUnitChange += UnitManager_OnSelectedUnitChange;
        UpdateVisual();
    }

    private void UnitManager_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if(UnitManager.Instance.GetSelectedUnit() == _unit) // The selected unit is currently this unit
        {
            _renderer.enabled = true;                       // Turn on Selected Visual
        } else _renderer.enabled = false;                   // Otherwise turn off Selected Visual
    }
}
