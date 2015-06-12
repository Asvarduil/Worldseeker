using UnityEngine;
using System;
using System.Collections;

public enum CharacterRotationAxes
{
    MouseXAndY = 0,
    MouseX = 1,
    MouseY = 2
}

public enum PlayerActionStates
{
    Idle,
    Walk,
    Backstep,
    StrafeLeft,
    StrafeRight,
    Jump,
    FireWeapon
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
    public PlayerActionStates ActionState = PlayerActionStates.Idle;

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
        CheckWeaponFire();

        ActionState = GetActionState();

        CalculateMovement();
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

    private PlayerActionStates GetActionState()
    {
        var result = PlayerActionStates.Idle;

        float walkAmount = _controls.GetAxis(WalkAxis);
        float strafeAmount = _controls.GetAxis(StrafeAxis);

        bool jumping = _controls.GetAxisDown(JumpAxis);

        PlanarMotion = new Vector3(strafeAmount, 0, walkAmount);

        if (walkAmount > 0)
            result = PlayerActionStates.Walk;
        else if (walkAmount < 0)
            result = PlayerActionStates.Backstep;

        if (strafeAmount > 0)
            result = PlayerActionStates.StrafeRight;
        else
            result = PlayerActionStates.StrafeLeft;

        if (jumping)
            result = PlayerActionStates.Jump;

        return result;
    }

    private void CalculateMovement()
    {
        bool isJumping = (ActionState == PlayerActionStates.Jump)
            && JumpCount > 0
            && JumpLockout.CanAttempt();

        _movement.MoveCharacter(PlanarMotion, isJumping);

        if(isJumping)
        {
            JumpLockout.NoteLastOccurrence();
            JumpCount--;
        }

        if (_movement.IsGrounded)
        {
            JumpCount = MaxAllowedJumps;
        }
    }

    private void CalculateAnimation()
    {

    }

    #endregion Methods
}