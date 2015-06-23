using UnityEngine;
using System;
using System.Collections;

public enum CharacterRotationAxes
{
    MouseXAndY = 0,
    MouseX = 1,
    MouseY = 2
}

public class PlayerControlSystem : DebuggableBehavior
{
    #region Variables / Properties

    public CharacterRotationAxes MouseLookAxes = CharacterRotationAxes.MouseXAndY;

    public string WalkAxis;
    public string StrafeAxis;
    public string JumpAxis;
    public string FireAxis;

    public int JumpCount = 1;
    public int MaxAllowedJumps = 1;
    public Lockout JumpLockout;

    public Vector3 PlanarMotion = Vector3.zero;

    private ControlManager _controls;

    private WeaponControls _weapon;
    private MovementControls _movement;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _controls = ControlManager.Instance;

        _weapon = GetComponentInChildren<WeaponControls>();
        _movement = GetComponent<MovementControls>();
    }

    public void Update()
    {
        CalculateRotation();
        GetInput();
        PerformMovement();
        CalculateAnimation();
    }

    #endregion Hooks

    #region Methods

    private void CalculateRotation()
    {
        switch (MouseLookAxes)
        {
            case CharacterRotationAxes.MouseXAndY:
                Vector2 axialRotation = _controls.GetMouse();
                _movement.LookInDirection(axialRotation);
                break;

            case CharacterRotationAxes.MouseX:
                _movement.RotateCharacterY(_controls.GetMouse().x);
                break;

            case CharacterRotationAxes.MouseY:
                _movement.RotateLookBoneX(_controls.GetMouse().y);
                break;

            default:
                throw new InvalidOperationException("Unexpected Character Rotation Axis: " + MouseLookAxes);
        }
    }

    private void CheckWeaponFire()
    {
        if (!_controls.GetAxisDown(FireAxis))
            return;

        _weapon.Fire();
    }

    private void GetInput()
    {
        CheckWeaponFire();

        float walkAmount = _controls.GetAxis(WalkAxis);
        float strafeAmount = _controls.GetAxis(StrafeAxis);

        PlanarMotion = new Vector3(strafeAmount, 0, walkAmount);
        _movement.ProcessPlanarMovement(PlanarMotion);

        bool jumpPressed = _controls.GetAxisDown(JumpAxis);
        bool jumpHeld = (_controls.GetAxis(JumpAxis) > 0.0f) && !jumpPressed;
        bool jumpReleased = _controls.GetAxisUp(JumpAxis);

        if (jumpPressed
            && !_movement.IsJumping
            && JumpCount > 0
            && JumpLockout.CanAttempt())
        {
            _movement.PerformJump();
            JumpCount--;
            JumpLockout.NoteLastOccurrence();
        }
            
        if (jumpHeld && _movement.IsJumping)
        {
            _movement.PerformJump();
        }

        if(jumpReleased && _movement.IsJumping)
        {
            _movement.AbortJump();
        }
        
        //result = PlayerActionStates.Idle;

        //if (walkAmount > 0)
        //    result = PlayerActionStates.Walk;
        //else if (walkAmount < 0)
        //    result = PlayerActionStates.Backstep;

        //if (strafeAmount > 0)
        //    result = PlayerActionStates.StrafeRight;
        //else if (strafeAmount < 0)
        //    result = PlayerActionStates.StrafeLeft;
    }

    private void PerformMovement()
    {
        _movement.ApplyVelocity(true);

        if (_movement.IsGrounded)
        {
            JumpCount = MaxAllowedJumps;
        }
    }

    private void CalculateAnimation()
    {
        // TODO: ...
    }

    #endregion Methods
}