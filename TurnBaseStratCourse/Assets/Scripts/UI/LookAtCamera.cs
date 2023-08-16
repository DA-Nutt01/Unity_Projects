using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Attach this to WorldUI Parent Obj in Unit Prefab

    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate() 
    {
        Vector3 lookDirection = (_cameraTransform.position - transform.position).normalized;

        // Normally the World UI is backwards when looking at the cam, so this inverts it 
        transform.LookAt(transform.position + lookDirection * -1);
    }
}
