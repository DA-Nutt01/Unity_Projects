using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Tooltip("Target position this unit will move towards.")]
    private Vector3 _targetPostion;

    [SerializeField, Tooltip("Multiplier for movement speed")]
    private int _moveSpeed = 4;

    [SerializeField, Tooltip("Animator component of this unit")]
    private Animator _unitAnimator;

    void Update()
    {
        float stoppinDistance = .1f;

        if (Vector3.Distance(_targetPostion, transform.position) > stoppinDistance)
        {
            Vector3 moveDirection = (_targetPostion - transform.position).normalized; // Calculate direction to move in
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;        // Move Logic
            _unitAnimator.SetBool("IsWalking", true);                                 // Animate Unit while moving
        } else _unitAnimator.SetBool("IsWalking", false);                             // Update animations when no longer moving
        
        if (Input.GetMouseButtonDown(0)) 
        {
            Move(MouseController.GetMousePosition());
        }
    }

    private void Move(Vector3 targetPosition) 
    {
        _targetPostion = targetPosition;
    }
}
