using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    // Attach this to the Empty CameraManager GameObject

    [SerializeField, Tooltip("The virtural camera responsible for cinematic shots while a unit is shooting")] 
    private GameObject _actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        // We need to check which action fired this event to know whether the action camera will activate or not

        switch (sender)
        {
            case ShootAction shootAction:
                // Cache shooter & target to calculate camera postions & look dir
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                // Calculate Action Camera position
                Vector3 cameraCharacterHeight = Vector3.up * 1.6f; // 1.7 rep the world units up to be at the standard unit's shoudler; keep in mind when getting the positions of units, it starts at their feet
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                float shoulderOffsetAmount = .75f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;
                Vector3 actionCameraPosition =
                    shooterUnit.GetWorldPosition() +
                    cameraCharacterHeight +
                    shoulderOffset +
                    (shootDir * -1);


                // Position  & Angle Action Camera
                _actionCameraGameObject.transform.position = actionCameraPosition;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                // Enable Action Camera
                ShowActionCamera();
                break;
            default:
                break;
        }
    }

    private  void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        HideActionCamera();
    }
}
