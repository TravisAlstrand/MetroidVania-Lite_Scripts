using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerManager))]
public class PlayerStateMachine : MonoBehaviour
{
  // GROUNDED STATES
  public IdleState _idleState = new();
  public MovingState _movingState = new();
  public LandingState _landingState = new();
  public WallSlidingState _wallSlidingState = new();

  // AIRBORNE STATES
  public JumpingState _jumpingState = new();
  public FallingState _fallingState = new();
  public WallJumpingState _wallJumpingState = new();
  public SmallFallingState _smallFallingState = new();

  // SMALL STATES
  public ShrinkingState _shrinkingState = new();
  public SmallIdleState _smallIdleState = new();
  public SmallMovingState _smallMovingState = new();
  public GrowingState _growingState = new();

  // OTHER STATES
  public DashingState _dashingState = new();
  public ShieldedState _shieldedState = new();
  public FiringState _firingState = new();
  public AttackingState _attackingState = new();

  // CURRENT STATE
  public PlayerBaseState CurrentState { get; private set; }

  // COMPONENTS
  private PlayerManager _player;

  private void Awake()
  {
    _player = GetComponent<PlayerManager>();
  }

  private void Start()
  {
    SwitchState(_idleState);
  }

  private void Update()
  {
    CurrentState?.UpdateState(this, _player);
    // Debug.Log(CurrentState);
  }

  private void FixedUpdate()
  {
    CurrentState?.FixedUpdateState(this, _player);
  }

  public void SwitchState(PlayerBaseState newState)
  {
    // Debug.Log($"Switching from {CurrentState} to {newState}");
    CurrentState?.ExitState(this, _player);
    CurrentState = newState;
    CurrentState.EnterState(this, _player);
  }
}
