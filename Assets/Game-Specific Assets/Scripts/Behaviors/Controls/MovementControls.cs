using System;
using System.Collections;
using UnityEngine;

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
    public float JumpSmoothing = 0.25f;
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
        get 
        { 
            bool isGrounded = _characterController.isGrounded;
            if (isGrounded && MovementType != MovementType.Grounded)
                MovementType = MovementType.Grounded;

            return isGrounded;
        }
    }

    public bool IsJumping
    {
        get { return MovementType == MovementType.Jumping; }
    }

    public bool IsFalling
    {
        get 
        {
            if (!IsGrounded && MovementType != MovementType.Falling)
                MovementType = MovementType.Falling;

            return MovementType == MovementType.Falling; 
        }
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

    public void ApplyVelocity(bool useDeltaTime = true)
    {
        if(useDeltaTime)
            _velocity *= Time.deltaTime;

        _collision = _characterController.Move(_velocity);

        if (MovementType == MovementType.Jumping && HitCeiling)
        {
            AbortJump();
        }

        if(IsGrounded)
        {
            _velocity.y = 0.0f;
            MovementType = MovementType.Grounded;
        }
    }

    public void ProcessPlanarMovement(Vector3 directionalInput)
    {
        if (!CanControl)
            return;

        _velocity = (transform.TransformDirection(directionalInput) * WalkSpeed);
    }

    public void PerformJump()
    {
        switch (MovementType)
        {
            case MovementType.Grounded:
            case MovementType.Falling:
                DebugMessage("Jumping...");
                _velocity.y += JumpForce;
                MovementType = MovementType.Jumping;
                break;

            case MovementType.Jumping:
                DebugMessage("Ascending...");
                _velocity.y *= JumpSmoothing;
                if(Mathf.Abs(_velocity.y - 0.0f) < 0.001f)
                {
                    AbortJump();
                }
                break;

            default:
                throw new InvalidOperationException("Unexpected movement state: " + MovementType);
        }
    }

    private void AbortJump()
    {
        _velocity.y = 0.0f;
        MovementType = MovementType.Falling;

        DebugMessage(gameObject.name + " is now falling.");
    }

    #region Rotation

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

    #endregion Rotation

    #endregion Methods
}
