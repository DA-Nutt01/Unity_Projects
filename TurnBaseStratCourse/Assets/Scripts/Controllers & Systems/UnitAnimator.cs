using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _unitAnimator;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving  += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += shootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _unitAnimator.SetBool("IsWalking", true);
    }

    private void shootAction_OnShoot(object sender, EventArgs e)
    {
        _unitAnimator.SetTrigger("Shoot");
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _unitAnimator.SetBool("IsWalking", false);
    }
}
