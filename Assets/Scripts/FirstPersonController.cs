using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller to handle looking around in a first person camera.
///
/// This controller expects the <see cref="playerObject"/> to be the container of the <see cref="playerCamera"/>.
/// When looking around with a mouse, the <see cref="playerObject"/> will follow the yaw of the player, and the
/// <see cref="playerCamera"/> will follow the pitch of the player.
/// </summary>
[AddComponentMenu("Rogue/First Person Controller")]
public class FirstPersonController : MonoBehaviour
{
    /// <summary>
    /// The <see cref="GameObject"/> that contains the player.
    /// </summary>
    public GameObject playerObject;

    /// <summary>
    /// The <see cref="Camera"/> that the player views the world through.
    /// </summary>
    public Camera playerCamera;

    /// <summary>
    /// The mouse sensitivity on the Y axis.
    /// </summary>
    public float verticalMouseSensitivity = 100f;

    /// <summary>
    /// The mouse sensitivity on the X axis.
    /// </summary>
    public float horizontalMouseSensitivity = 100f;

    /// <summary>
    /// Indicates if the vertical mouse movement should be inverted.
    /// </summary>
    public bool invertVerticalMouseMovement = false;

    /// <summary>
    /// The maximum vertical angle at which the player can look at.
    /// </summary>
    [Range(-90f, 90f)]
    public float maximumVerticalMouseAngle = 90f;

    /// <summary>
    /// The minimum vertical angle at which the player can look at.
    /// </summary>
    [Range(-90f, 90f)]
    public float minimumVerticalMouseAngle = -90f;

    private CursorLockMode _previousCursorLockMode;
    
    private float _xRotation;

    private void Start()
    {
        _xRotation = (maximumVerticalMouseAngle - minimumVerticalMouseAngle) / 2 + minimumVerticalMouseAngle;
    }
    
    private void OnEnable()
    {
        _previousCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = _previousCursorLockMode;
    }

    private void Update()
    {
        var xMouseDelta = Input.GetAxis("Mouse X") * horizontalMouseSensitivity;
        var yMouseDelta = Input.GetAxis("Mouse Y") * verticalMouseSensitivity;

        _xRotation -= (invertVerticalMouseMovement ? -yMouseDelta : yMouseDelta) * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, minimumVerticalMouseAngle, maximumVerticalMouseAngle);
        
        playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerObject.transform.Rotate(Vector3.up, xMouseDelta * Time.deltaTime);
    }
}
