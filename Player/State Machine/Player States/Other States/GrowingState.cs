using UnityEngine;

public class GrowingState : PlayerBaseState
{
  private readonly string _animationName = "Growing";
  private float _animationLength;
  private float _stateTimer;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
    AnimationClip clip = player.GetClipByName(_animationName);
    _animationLength = clip.length;
    _stateTimer = _animationLength;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _stateTimer -= Time.deltaTime;

    if (_stateTimer <= 0f)
    {
      if (player.FrameInput.Move.x != 0f)
      {
        stateM.SwitchState(stateM._movingState);
        return;
      }
      else
      {
        stateM.SwitchState(stateM._idleState);
      }
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;

    if (player.IsOnWaterCannotSwim)
    {
      player.Rigidbody.linearVelocityY = 0f;
      player.Rigidbody.AddForceY(player.WaterBumpCannotSwim);
      player.IsOnWaterCannotSwim = false;
    }
  }

  public override void ExitState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.SwitchBodyColliders();
    player.IsSmall = false;
  }
}
