public class SmallMovingState : PlayerBaseState
{
  private readonly string _animationName = "SmallMoving";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.IsGrounded)
    {
      if (player.CanGrow)
      {
        stateM.SwitchState(stateM._growingState);
        return;
      }

      if (player.FrameInput.Move.x == 0f)
      {
        stateM.SwitchState(stateM._smallIdleState);
      }
    }
    else
    {
      stateM.SwitchState(stateM._fallingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.SmallMoveSpeed;
  }
}
