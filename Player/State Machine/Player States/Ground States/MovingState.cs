public class MovingState : PlayerBaseState
{
  private readonly string _animationName = "Moving";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.CanAttack)
    {
      stateM.SwitchState(stateM._attackingState);
      return;
    }

    if (player.CanDash)
    {
      stateM.SwitchState(stateM._dashingState);
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
    }

    if (player.IsGrounded)
    {
      if (player.CanJump)
      {
        stateM.SwitchState(stateM._jumpingState);
        return;
      }

      if (player.FrameInput.Move.x == 0f)
      {
        stateM.SwitchState(stateM._idleState);
        return;
      }
    }
    else
    {
      if (player.CanJump)
      {
        stateM.SwitchState(stateM._jumpingState);
        return;
      }

      stateM.SwitchState(stateM._fallingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;

    if (player.IsOnWaterCannotSwim)
    {
      player.Rigidbody.linearVelocityY = 0f;
      player.Rigidbody.AddForceY(player.WaterBumpCannotSwim);
      player.IsOnWaterCannotSwim = false;
    }
  }
}
