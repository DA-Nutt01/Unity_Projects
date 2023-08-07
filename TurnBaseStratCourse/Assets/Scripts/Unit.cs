using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Tooltip("Target world position this unit will move towards.")]
    private Vector3 _targetPostion;

    [SerializeField, Tooltip("READONLY: The current GridPosition this unit is occupying")]
    private GridPosition _currentGridPosition;

    [SerializeField, Tooltip("Multiplier for movement speed")]
    private int _moveSpeed = 4;

    [SerializeField, Tooltip("Multiplier for rotating speed when unit is turning")]
    private float _rotateSpeed = 10f;

    [SerializeField, Tooltip("Animator component of this unit")]
    private Animator _unitAnimator;

    void Awake() 
    {
        _targetPostion = transform.position; // Prevent Unit from wandering anywhere before issued a commmand
    }
    
    void Start()
    {
        _currentGridPosition =  LevelGrid.Instance.GetGridPosition(transform.position); // Cache this unit's starting GridPosition
        LevelGrid.Instance.AddUnitAtGridPosition(_currentGridPosition, this);           // Set the current position of this unit in it's current GridPosition

    }

    void Update()
    {
        float stoppinDistance = .1f;

        if (Vector3.Distance(_targetPostion, transform.position) > stoppinDistance)
        {
            Vector3 moveDirection = (_targetPostion - transform.position).normalized;                          // Calculate direction to move in
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;                                 // Move Logic
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed); // Rotate unit model to face dir it is moving in
            _unitAnimator.SetBool("IsWalking", true);                                                          // Animate Unit while moving
        } else _unitAnimator.SetBool("IsWalking", false);                                                      // Update animations when no longer moving

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);                 // Recalculate the GridPosition of this unit each frame
        if (_currentGridPosition != newGridPosition)                                                           // If this unit changed GridPosition
        {
            // This unit has changed its GridPosition
            LevelGrid.Instance.ChangeUnitGridPosition(this, _currentGridPosition, newGridPosition);            // Change the current GridPosition of this Unit
            _currentGridPosition = newGridPosition;                                                            // Update the Unit's current GridPosition
        }
    }

    public void Move(Vector3 targetPosition) 
    {
        _targetPostion = targetPosition;
    }
}
