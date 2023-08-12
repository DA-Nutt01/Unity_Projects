using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;
    private void Update()
    {
        if (!_isActive) return; // Skip all other code while this action is not active

        float spinDegrees = 360f * Time.deltaTime; // The number of degress the unit will spin
        transform.eulerAngles += new Vector3(0, spinDegrees, 0);

        _totalSpinAmount += spinDegrees;
        if (_totalSpinAmount > 360f) 
        {
            _isActive = false; // Stop spinning once unit has spun 360 degrees
            _onActionComplete(); // Call our delegate from the base class
        }
    }
    public void Spin(Action OnSpinComplete) // This method takes in a delegate as a parameter; When calling Spin, it now needs a function to store that it will call once it is complete
    {
        this._onActionComplete = OnSpinComplete; // Take the argument SpinCompleteDelegate and set the internal _onSpinComplete delegate as it
        _isActive = true;
        _totalSpinAmount = 0; 
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
