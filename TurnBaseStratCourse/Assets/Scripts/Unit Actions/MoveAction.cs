using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction
{

    [SerializeField, Tooltip("Target world position this unit will move towards.")]
    private Vector3 _targetPostion;

    [SerializeField, Tooltip("Animator component of this unit")]
    private Animator _unitAnimator;

    [SerializeField, Tooltip("The max number of grid positions this unit can travel in a single move action")]
    private int _maxMoveDistance = 4;
    
    [SerializeField, Tooltip("Multiplier for movement speed of this unit")]
    private int _moveSpeed = 4;

    [SerializeField, Tooltip("Multiplier for rotating speed when unit is turning")]
    private float _rotateSpeed = 10f;

    protected override void Awake() 
    {
        base.Awake();                        // Call the Awake() on the parent class for this classs
        _targetPostion = transform.position; // Prevent Unit from wandering anywhere before issued a commmand
    }

    private void Update()
    {
        if (!_isActive) return; // While this action is not allowed to run, skip all below code and start a new iteration of Update()

        float stoppinDistance = .1f;
        Vector3 moveDirection = (_targetPostion - transform.position).normalized;                             // Calculate direction to move in

        if (Vector3.Distance(_targetPostion, transform.position) > stoppinDistance)
        {
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;                                 // Move Logic
            _unitAnimator.SetBool("IsWalking", true);                                                          // Animate Unit while moving
        }
        else
        {
            _unitAnimator.SetBool("IsWalking", false);                                                      // Update animations when no longer moving
            ActionComplete();                                                                            // Call the delegate function to clear _isBusy on The UnitActionSystem
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed); // Rotate unit model to face dir it is moving in
    }
    public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete) 
    {
        ActionStart(onActionComplete);
        _targetPostion = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // Returns a list of  valid GridPositions that are within this unit's max move distance
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++) 
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // If the GridPosition is not a valid one or is the unit's current GridPosition, or is currently occupied by another unit
                {
                    if (!LevelGrid.Instance.isValidGridPosition(testGridPosition) || testGridPosition == unitGridPosition || LevelGrid.Instance.isGridPositionOccupied(testGridPosition)) 
                    continue; // Skip ahead to the next iteration of this loop (do not add this GridPosition to the list of valid ones
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
