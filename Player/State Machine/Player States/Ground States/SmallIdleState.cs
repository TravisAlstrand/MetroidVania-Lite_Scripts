public class SmallIdleState : PlayerBaseState
{
  private readonly string _animationName = "SmallIdle";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
    player.Rigidbody.linearVelocityX = 0f;
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

      if (player.FrameInput.Move.x != 0f)
      {
        stateM.SwitchState(stateM._smallMovingState);
        return;
      }
    }
    else
    {
      stateM.SwitchState(stateM._smallFallingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.IsOnWaterCannotSwim)
    {
      player.Rigidbody.linearVelocityY = 0f;
      player.Rigidbody.AddForceY(player.WaterBumpCannotSwim);
      player.IsOnWaterCannotSwim = false;
    }
  }
}
