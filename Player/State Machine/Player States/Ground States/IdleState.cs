using UnityEngine;

public class IdleState : PlayerBaseState
{
  private readonly string _animationName = "Idle";
  private readonly float _timeUntilCanLookDown = .5f;
  private float _lookDownTimer;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    _lookDownTimer = _timeUntilCanLookDown;
    player.Animator.Play(_animationName);
    player.Rigidbody.linearVelocityX = 0f;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _lookDownTimer -= Time.deltaTime;

    if (player.CanJump)
    {
      stateM.SwitchState(stateM._jumpingState);
      return;
    }

    if (player.CanAttack)
    {
      stateM.SwitchState(stateM._attackingState);
      return;
    }

    if (player.CanDash)
    {
      stateM.SwitchState(stateM._dashingState);
      return;
    }

    if (player.CanShrink)
    {
      stateM.SwitchState(stateM._shrinkingState);
      return;
    }

    if (player.CanShield)
    {
      stateM.SwitchState(stateM._shieldedState);
      return;
    }

    if (player.CanFire)
    {
      stateM.SwitchState(stateM._firingState);
      return;
    }

    if (player.IsUnderWater)
    {
      stateM.SwitchState(stateM._swimmingState);
      return;
    }

    if (player.IsGrounded)
    {
      if (player.FrameInput.Move.x != 0f)
      {
        stateM.SwitchState(stateM._movingState);
        return;
      }
    }
    else
    {
      stateM.SwitchState(stateM._fallingState);
      return;
    }

    if (_lookDownTimer <= 0f)
    {
      if (player.FrameInput.Move.y < 0f && !stateM.CameraManager.LookDownCameraActive)
      {
        stateM.CameraManager.ActivateLookDownCamera();
      }
      else if (player.FrameInput.Move.y >= 0f && stateM.CameraManager.LookDownCameraActive)
      {
        stateM.CameraManager.DeactivateLookDownCamera();
      }
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.IsOnWaterCannotSwim)
    {
      player.Rigidbody.AddForceY(player.WaterBumpCannotSwim);
      player.IsOnWaterCannotSwim = false;
    }
  }

  public override void ExitState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (stateM.CameraManager.LookDownCameraActive)
    {
      stateM.CameraManager.DeactivateLookDownCamera();
    }
  }
}
