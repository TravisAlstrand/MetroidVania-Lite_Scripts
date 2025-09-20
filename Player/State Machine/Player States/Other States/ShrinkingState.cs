public class ShrinkingState : PlayerBaseState
{
  private readonly string _animationName = "Shrinking";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }
}
