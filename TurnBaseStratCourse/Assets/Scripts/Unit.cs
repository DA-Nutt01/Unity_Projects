using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Tooltip("Target position this unit will move towards.")]
    private Vector3 _targetPostion;

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
    }

    public void Move(Vector3 targetPosition) 
    {
        _targetPostion = targetPosition;
    }
}
