using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Tooltip("Target position this unit will move towards.")]
    private Vector3 _targetPostion;

    [SerializeField, Tooltip("Multiplier for movement speed")]
    private int _moveSpeed = 4;

    void Update()
    {
        float stoppinDistance = .1f;

        if (Vector3.Distance(_targetPostion, transform.position) > stoppinDistance)
        {
            Vector3 moveDirection = (_targetPostion - transform.position).normalized;

            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        }
        
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
