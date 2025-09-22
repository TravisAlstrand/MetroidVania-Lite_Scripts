using UnityEngine;

public class WallSlidingState : PlayerBaseState
{
  private readonly string _animationName = "OnWall";
  private float _wallStickTimer = 0.125f;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.ChangeSpriteColor(player.WallColor);
    player.Animator.Play(_animationName);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _wallStickTimer -= Time.deltaTime;

    if (player.CanWallJump)
    {
      stateM.SwitchState(stateM._wallJumpingState);
      return;
    }

    if (player.IsGrounded)
    {
      stateM.SwitchState(stateM._idleState);
      return;
    }

    if (!player.IsGrounded && !player.IsOnWall)
    {
      stateM.SwitchState(stateM._fallingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    // PREVENT DOWNWARD WALL SLIDING FOR SHORT PERIOD
    if (_wallStickTimer > 0f) return;

    if (player.Rigidbody.linearVelocityY <= 0f)
    {
      player.Rigidbody.linearVelocityY = -player.WallSlideSpeed;
    }
    else
    {
      player.Rigidbody.linearVelocityY = Mathf.Min(player.Rigidbody.linearVelocityY, 2f);
    }
  }
}
