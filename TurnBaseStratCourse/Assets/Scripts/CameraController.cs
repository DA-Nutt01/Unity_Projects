using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // This Script must be attached to the empty game object the main virtual camera is to track
    // Manipulation of the camera movement & rotation will be achived by moving/rotating this ref game obj

    [SerializeField, Tooltip("Speed mulitplier for the camera movement")]
    private float _moveSpeed = 10f;

    [SerializeField, Tooltip("Speed mulitplier for the camera rotation")]
    private float _rotationSpeed = 100f;

    void Update()
    {
        // Movement
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.W)) // Forward
        {
            inputMoveDirection.z = 1f;
        }

        if(Input.GetKey(KeyCode.A)) // Left
        {
            inputMoveDirection.x = -1f;
        }

        if(Input.GetKey(KeyCode.S)) // Backward
        {
            inputMoveDirection.z = -1f;
        }

        if(Input.GetKey(KeyCode.D)) // Right
        {
            inputMoveDirection.x = 1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x; // Calculate movement Vector for the camera each frame
        transform.position += moveVector * _moveSpeed * Time.deltaTime;                                         // Apply the moveVector to the camera each frame

        // Rotation
        Vector3 rotationVector = new Vector3(0 , 0, 0);

        if(Input.GetKey(KeyCode.Q)) // Rotate Right
        {
            rotationVector.y = 1f;
        }

        if(Input.GetKey(KeyCode.E)) // Rotate Left
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += rotationVector * _rotationSpeed * Time.deltaTime; //Apply rotation input to rotation of the camera
    }
}
