using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
  public FrameInput FrameInput { get; private set; }

  private PlayerInputActions _playerInputActions;
  private InputAction _move;
  private InputAction _jump;
  private InputAction _attack;
  private InputAction _dash;
  private InputAction _shrink;
  private InputAction _shield;
  private InputAction _fire;

  private void Awake()
  {
    _playerInputActions = new PlayerInputActions();
    _move = _playerInputActions.Player.Move;
    _jump = _playerInputActions.Player.Jump;
    _attack = _playerInputActions.Player.Attack;
    _dash = _playerInputActions.Player.Dash;
    _shrink = _playerInputActions.Player.Shrink;
    _shield = _playerInputActions.Player.Shield;
    _fire = _playerInputActions.Player.Fire;
  }

  private void OnEnable()
  {
    _playerInputActions.Enable();
  }

  private void OnDisable()
  {
    _playerInputActions.Disable();
  }

  private void Update()
  {
    FrameInput = GatherInput();
  }

  private FrameInput GatherInput()
  {
    return new FrameInput
    {
      Move = _move.ReadValue<Vector2>(),
      Jump = _jump.WasPressedThisFrame(),
      JumpHeld = _jump.inProgress,
      Attack = _attack.WasPressedThisFrame(),
      Dash = _dash.WasPressedThisFrame(),
      ShrinkGrow = _shrink.WasPressedThisFrame(),
      Shield = _shield.WasPressedThisFrame(),
      Fire = _fire.WasPressedThisFrame()
    };
  }
}

public struct FrameInput
{
  public Vector2 Move;
  public bool Jump;
  public bool JumpHeld;
  public bool Attack;
  public bool Dash;
  public bool ShrinkGrow;
  public bool Shield;
  public bool Fire;
}
