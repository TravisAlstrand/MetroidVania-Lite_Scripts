using UnityEngine;

public class DashingState : PlayerBaseState
{
  private readonly string _animationName = "Dashing";
  private float _dashDurationTimer;
  private float _dashDirection;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
    player.ChangeSpriteColor(player.DashColor);
    _dashDurationTimer = player.DashDuration;

    _dashDirection = player.IsFacingRight ? 1f : -1f;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _dashDurationTimer -= Time.deltaTime;

    if (_dashDurationTimer <= 0f)
    {
      if (player.IsGrounded)
      {
        if (player.FrameInput.Move.x != 0f)
        {
          stateM.SwitchState(stateM._movingState);
          return;
        }
        else
        {
          stateM.SwitchState(stateM._idleState);
          return;
        }
      }
      else
      {
        stateM.SwitchState(stateM._fallingState);
        return;
      }
    }

    if (player.IsOnWall && player.WallAbilitiesUnlocked && !player.IsGrounded)
    {
      stateM.SwitchState(stateM._wallSlidingState);
      return;
    }

    if (player.IsOnWall && !player.WallAbilitiesUnlocked && !player.IsGrounded)
    {
      stateM.SwitchState(stateM._fallingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (_dashDurationTimer > 0f)
    {
      player.Rigidbody.linearVelocity = new Vector2(
        _dashDirection * player.DashSpeed,
        0f
      );
    }
  }

  public override void ExitState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.StartDashCoolDown();
  }
}
