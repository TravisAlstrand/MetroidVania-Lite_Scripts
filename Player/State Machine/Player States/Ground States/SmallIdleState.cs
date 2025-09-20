public class SmallIdleState : PlayerBaseState
{
  private readonly string _animationName = "SmallIdle";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }
}
