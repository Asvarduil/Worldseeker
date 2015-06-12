using UnityEngine;
using System.Collections;

public enum MovementType
{
    Grounded,
    Jumping,
    Falling
}

[RequireComponent(typeof(CharacterController))]
public class MovementControls : DebuggableBehavior, ISuspendable 
{
    #region Variables / Properties

    public bool CanControl = true;

    public float WalkSpeed = 10.0f;
    public float JumpForce = 20.0f;
    public float JumpSmoothing = 0.95f;
    public MovementType MovementType = MovementType.Grounded;
       
    // Mouse Look variables...
    public GameObject VerticalLookBone;
    public Vector2 axisSensitivity = new Vector2(45.0f, 15.0f);
    public Vector2 minRotationBounds = new Vector2(-360.0f, -60.0f);
    public Vector2 maxRotationBounds = new Vector2(360.0f, 60.0f);

    private Vector2 _currentRotation = new Vector2(0, 0);

    // Movement variables...
    private Vector3 _velocity = Vector3.zero;
    private CollisionFlags _collision;

    // Components...
    private CharacterController _characterController;

    // Properties...
    public bool HitCeiling
    {
        get { return (_collision & CollisionFlags.CollidedAbove) != CollisionFlags.None; }
    }

    public bool IsGrounded
    {
        get { return _characterController.isGrounded; }
    }
    
    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    public void Suspend()
    {
        CanControl = false;
        HaltMotion();
    }

    public void Resume()
    {
        CanControl = true;
    }

    #endregion Hooks

    #region Methods

    public void HaltMotion()
    {
        _velocity = Vector3.zero;
    }

    private void ApplyVelocity()
    {
        _collision = _characterController.Move(_velocity);
    }

    public void MoveCharacter(Vector3 directionalInput, bool performJump, bool useDeltaTime = true)
    {
        if (!CanControl)
            return;

        float verticalForce = _velocity.y;

        ApplyDirectionalInput(directionalInput);

        _velocity.y = verticalForce;
        if (performJump)
            PerformJump();
        else if (MovementType == MovementType.Jumping && !performJump)
            AbortJump();

        ApplyVerticalForces();

        if(useDeltaTime)
            _velocity *= Time.deltaTime;

        ApplyVelocity();
    }

    private void ApplyDirectionalInput(Vector3 directionalInput)
    {
        _velocity = (transform.TransformDirection(directionalInput) * WalkSpeed);
    }

    private void PerformJump()
    {
        _velocity.y += JumpForce;
        MovementType = MovementType.Jumping;

        DebugMessage(gameObject.name + " is now jumping.");
    }

    private void AbortJump()
    {
        _velocity.y = 0.0f;
        MovementType = MovementType.Falling;

        DebugMessage(gameObject.name + " is now falling.");
    }

    /// <summary>
    /// Applies jump smoothing and gravity to the given velocity.
    /// </summary>
    /// <param name="velocity">Velocity to apply vertical forces to.</param>
    /// <returns>Modified velocity</returns>
    private void ApplyVerticalForces()
    {       
        Vector3 modifiedVelocity = _velocity;

        // If jumping, smooth the jump...
        // Check that we haven't hit our head.
        // Check that we haven't decelerated to a fall in a jump.
        if (MovementType == MovementType.Jumping)
        {
            if (!HitCeiling)
            {
                modifiedVelocity.y *= JumpSmoothing;

                if (Mathf.Abs(modifiedVelocity.y - 0.0f) < 0.001)
                {
                    DebugMessage(gameObject.name + " at top of jump; now falling.");
                    MovementType = MovementType.Falling;
                }
            }
            else
            {
                AbortJump();
            }
        }

        // Check that we haven't hit the ground.
        if (MovementType == MovementType.Falling && IsGrounded)
        {
            modifiedVelocity.y = 0.0f;
            MovementType = MovementType.Grounded;
        }

        // Apply gravity at all times.
        modifiedVelocity = modifiedVelocity + Physics.gravity;
        DebugMessage(gameObject.name + " has a vertical velocity of " + modifiedVelocity.y + " after gravity is applied.");

        _velocity = modifiedVelocity;
    }

    /// <summary>
    /// Causes the Camera to look in the direction of the given rotation.
    /// </summary>
    /// <param name="axialRotation">The two-component rotation to look towards.</param>
    public void LookInDirection(Vector2 axialRotation)
    {
        RotateLookBoneX(axialRotation.y);
        RotateCharacterY(axialRotation.x);
    }

    /// <summary>
    /// Rotates the character along its Y axis (horizontal rotation)
    /// </summary>
    /// <param name="rotation">Amount by which to rotate the character.</param>
    public void RotateCharacterY(float rotation)
    {
        _currentRotation.x = transform.localEulerAngles.y + rotation * axisSensitivity.x;
        transform.localEulerAngles = new Vector3(0, _currentRotation.x, 0);
    }

    /// <summary>
    /// Rotates the Vertical Look Bone along its X axis (vertical rotation)
    /// </summary>
    /// <param name="rotation">Amount by which to rotate the Vertical Look Bone.</param>
    public void RotateLookBoneX(float rotation)
    {
        if (VerticalLookBone == null)
            return;

        _currentRotation.y = rotation * axisSensitivity.y;
        _currentRotation.y = Mathf.Clamp(_currentRotation.y, minRotationBounds.y, maxRotationBounds.y);

        VerticalLookBone.transform.Rotate(-_currentRotation.y, 0, 0);
    }

    #endregion Methods
}
