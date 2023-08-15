using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // This Script must be attached to the empty game object the main virtual camera is to track
    // Manipulation of the camera movement & rotation will be achived by moving/rotating this ref game obj

    [SerializeField,Tooltip("Reference to main CM virtual camera following this game obj")]
    private CinemachineVirtualCamera _cmVirtualCam;

    private CinemachineTransposer _cmTransposerComponent;

    [Space(10),SerializeField, Tooltip("Speed mulitplier for the camera movement")]
    private float _moveSpeed = 10f;

    [SerializeField, Tooltip("Speed mulitplier for the camera rotation")]
    private float _rotationSpeed = 100f;

    [SerializeField, Tooltip("The amount of world units the camera position is adjusted every mouse scroll wheel tick")]
    private float _mouseScrollSensitivity = 1f;

    [SerializeField, Tooltip("Multiplier for how fast the cam will Lerp between new zoom positions")]
    private float _zoomSpeed = 5f;

    private Vector3 _targetFollowOffset; // The offset position of the cam from this follow target

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    private void Start() 
    {
        _cmTransposerComponent = _cmVirtualCam.GetCinemachineComponent<CinemachineTransposer>(); // Used to track the current target offset any given frame
        _targetFollowOffset = _cmTransposerComponent.m_FollowOffset;                             // The target offset the cam should position to when zooming
    }
    void Update()
    {
        HandleMovement(); 
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()

    {
        // Must be called in Update
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
    }
    private void HandleRotation() 
    {
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

    private void HandleZoom()
    {
        // The way zoom is handled here is not by literally zooming the camera but lowering it's position down on the Y-axis
        // Keep in mind while the rest of this script manages movement for the camera follow target, this method directly moves the cam

        if(Input.mouseScrollDelta.y > 0) // Scolling up --> Zoom In
        {
            _targetFollowOffset.y -= _mouseScrollSensitivity;
        }

        if(Input.mouseScrollDelta.y < 0) // Scolling down --> Zoom out
        {
            _targetFollowOffset.y += _mouseScrollSensitivity;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET); // Constrains the min/max zoom to the constants
        _cmTransposerComponent.m_FollowOffset = Vector3.Lerp(_cmTransposerComponent.m_FollowOffset, _targetFollowOffset, Time.deltaTime * _zoomSpeed);                                    // Applies the followOffset to the camera directly
    }
}
