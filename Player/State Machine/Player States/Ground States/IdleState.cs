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
    if (player.CanPerformJump())
    {
      stateM.SwitchState(stateM._jumpingState);
      return;
    }

    if (player.IsGrounded)
    {
      if (player.FrameInput.Move.x != 0f)
      {
        stateM.SwitchState(stateM._movingState);
        return;
      }

      return;
    }

    stateM.SwitchState(stateM._fallingState);
  }
}
