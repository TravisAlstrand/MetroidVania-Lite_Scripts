public class IdleState : PlayerBaseState
{
  private readonly string _animationName = "Idle";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
    player.Rigidbody.linearVelocityX = 0f;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.CanJump)
    {
      stateM.SwitchState(stateM._jumpingState);
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
    }
  }
}
