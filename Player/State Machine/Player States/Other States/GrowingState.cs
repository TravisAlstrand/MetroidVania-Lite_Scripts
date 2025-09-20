public class GrowingState : PlayerBaseState
{
  private readonly string _animationName = "Growing";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }
}
