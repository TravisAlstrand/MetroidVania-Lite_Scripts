public class SmallMovingState : PlayerBaseState
{
  private readonly string _animationName = "SmallMoving";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }
}
