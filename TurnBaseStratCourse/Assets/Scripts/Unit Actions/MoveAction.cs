using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Unit _unit; // Reference to the Unit Component of this Unit; Every action will need this reference

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

        void Awake() 
    {
        _targetPostion = transform.position; // Prevent Unit from wandering anywhere before issued a commmand
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        float stoppinDistance = .1f;

        if (Vector3.Distance(_targetPostion, transform.position) > stoppinDistance)
        {
            Vector3 moveDirection = (_targetPostion - transform.position).normalized;                          // Calculate direction to move in
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;                                 // Move Logic
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed); // Rotate unit model to face dir it is moving in
            _unitAnimator.SetBool("IsWalking", true);                                                          // Animate Unit while moving
        } else _unitAnimator.SetBool("IsWalking", false);                                                      // Update animations when no longer moving

    }
        public void Move(GridPosition targetGridPosition) 
    {
        _targetPostion = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        // Takes in a GridPosition & returns true if is a valid GridPosition within the Unit's movement range
        List<GridPosition> validGridPositionsInMoveRange = GetValidActionGridPositionList(); // Get the list of valid GridPositions within the unit's move range
        return validGridPositionsInMoveRange.Contains(gridPosition); // Return true if the given GridPosition is within the list of valid GridPositions in move rage
    }

    public List<GridPosition> GetValidActionGridPositionList()
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
}
