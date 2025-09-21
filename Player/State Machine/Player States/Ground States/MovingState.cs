public class MovingState : PlayerBaseState
{
  private readonly string _animationName = "Moving";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.CanDash)
    {
      stateM.SwitchState(stateM._dashingState);
    }

    if (player.IsGrounded)
    {
      if (player.CanPerformJump())
      {
        stateM.SwitchState(stateM._jumpingState);
        return;
      }

      if (player.CanShrink)
      {
        stateM.SwitchState(stateM._shrinkingState);
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
      if (player.CanPerformJump())
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
  }
}
