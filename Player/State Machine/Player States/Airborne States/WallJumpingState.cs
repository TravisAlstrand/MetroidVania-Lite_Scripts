using UnityEngine;

public class WallJumpingState : PlayerBaseState
{
  private readonly string _animationName = "Jumping";
  private float _wallJumpTimer;
  private float _wallJumpDirection;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);

    _wallJumpDirection = player.IsFacingRight ? -1f : 1f;

    player.Rigidbody.AddForce(new Vector2(-player.transform.localScale.x * player.WallJumpForce.x, player.WallJumpForce.y), ForceMode2D.Impulse);

    _wallJumpTimer = player.MaxWallJumpTime;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _wallJumpTimer -= Time.deltaTime;

    // PREVENT IMMEDIATELY STICKING TO WALL AGAIN
    if (_wallJumpTimer <= 0f && player.IsOnWall)
    {
      stateM.SwitchState(stateM._wallSlidingState);
      return;
    }

    if (_wallJumpTimer <= 0f)
    {
      if (player.CanDash)
      {
        stateM.SwitchState(stateM._dashingState);
      }

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
        if (player.Rigidbody.linearVelocityY > 0f)
        {
          stateM.SwitchState(stateM._jumpingState);
          return;
        }
        else
        {
          stateM.SwitchState(stateM._fallingState);
          return;
        }
      }
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (_wallJumpTimer > 0f)
    {
      player.Rigidbody.linearVelocityX = _wallJumpDirection * player.WallJumpForce.x;
    }
    else
    {
      player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;
    }
  }
}
