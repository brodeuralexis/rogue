using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller to handle basic movement input for a player.
/// </summary>
[AddComponentMenu("Rogue/Movement Controller")]
public class MovementController : MonoBehaviour
{
    /// <summary>
    /// The <see cref="GameObject"/> that contains the <see cref="CharacterController"/> of the player.
    /// </summary>
    public CharacterController playerCharacterController;
    
    /// <summary>
    /// The movement speed of the player.
    /// </summary>
    public float movementSpeed = 12f;

    /// <summary>
    /// The vertical gravity that is to be applied to the player.
    /// </summary>
    public float gravity = -9.81f;

    /// <summary>
    /// The distance from the ground at which we considered to be on the ground.
    /// </summary>
    public float groundCheckDistance = 0.4f;

    /// <summary>
    /// The layer mask to which the ground check should apply.
    /// </summary>
    public LayerMask groundCheckMask;
    
    private Vector3 _velocity;
    
    private bool _grounded;
    
    private void Update()
    {
        var playerTransform = transform;
        var playerTransformUp = playerTransform.up;
        var forwardMovementDelta = Input.GetAxis("Vertical") * movementSpeed;
        var sidewaysMovementDelta = Input.GetAxis("Horizontal") * movementSpeed;
        
        _grounded = Physics.CheckSphere(playerTransform.position, groundCheckDistance, groundCheckMask);

        var upVelocity = Vector3.Dot(playerTransformUp, _velocity) / playerTransformUp.magnitude;
        
        if (_grounded && upVelocity < 0)
        {
            _velocity = Vector3.zero;
        }

        _velocity += playerTransform.forward * forwardMovementDelta +
                     playerTransform.right * sidewaysMovementDelta +
                     playerTransformUp * (gravity * Time.deltaTime);

        playerCharacterController.Move(_velocity * Time.deltaTime);

        // Only keep the upward/downward velocity.
        _velocity = Vector3.Project(_velocity, playerTransformUp);
    }
}
