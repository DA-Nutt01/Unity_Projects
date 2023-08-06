using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, Tooltip("Target position this unit will move towards.")]
    private Vector3 _targetPostion;

    [SerializeField, Tooltip("Multiplier for movement speed")]
    private int _moveSpeed = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float stoppinDistance = .1f;

        if (Vector3.Distance(_targetPostion, transform.position) > stoppinDistance)
        {
            Vector3 moveDirection = (_targetPostion - transform.position).normalized;

            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Move(new Vector3(Random.Range(-10, 11), 0, Random.Range(-10, 11)));
        }
    }

    private void Move(Vector3 targetPosition) 
    {
        _targetPostion = targetPosition;
    }
}
