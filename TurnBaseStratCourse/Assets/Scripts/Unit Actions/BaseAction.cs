using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    // Base Class for all actions 

    protected Unit _unit;               // Reference to the Unit Component of this Unit; Every action will need this reference
    protected bool _isActive;           // Flag for if this action is currently allowed to run or not
    protected Action _onActionComplete; // Delegate for when action is complete

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
