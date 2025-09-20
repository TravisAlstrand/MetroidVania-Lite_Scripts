public class PlayerBaseState
{
  public virtual void EnterState(PlayerStateMachine stateM, PlayerManager player) { }

  public virtual void UpdateState(PlayerStateMachine stateM, PlayerManager player) { }

  public virtual void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player) { }

  public virtual void ExitState(PlayerStateMachine stateM, PlayerManager player) { }
}
