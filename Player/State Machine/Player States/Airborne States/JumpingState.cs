using UnityEngine;

public class JumpingState : PlayerBaseState
{
  private readonly string _animationName = "Jumping";
  private bool _startedJumpTouchingWall = false;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    // PREVENT IMMEDIATE WALL SLIDING WHEN JUMPING TOUCHING WALL
    if (player.IsOnWall) _startedJumpTouchingWall = true;

    player._lastJumpTime = Time.time;

    player.Animator.Play(_animationName);
    player.Rigidbody.linearVelocityY = 0f;
    player.Rigidbody.AddForceY(player.JumpForce, ForceMode2D.Impulse);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    // VARIABLE JUMP HEIGHT CHECK
    if ((player.Rigidbody.linearVelocityY > 0f && !player.FrameInput.JumpHeld) ||
         player.Rigidbody.linearVelocityY <= 0f)
    {
      stateM.SwitchState(stateM._fallingState);
      return;
    }

    // BUFFERED / DOUBLE JUMP
    if (player.CanPerformJump())
    {
      stateM.SwitchState(stateM._jumpingState);
      return;
    }

    // LANDING CHECK
    if (player.IsGrounded)
    {
      stateM.SwitchState(stateM._landingState);
      return;
    }

    if (player.CanDash)
    {
      stateM.SwitchState(stateM._dashingState);
    }

    // WALL SLIDE
    if (player.IsOnWall && player.WallAbilitiesUnlocked)
    {
      if (_startedJumpTouchingWall)
      {
        if (player.Rigidbody.linearVelocityY > 0f)
        {
          return;
        }
      }
      else
      {
        if (!player.IsGrounded)
        {
          stateM.SwitchState(stateM._wallSlidingState);
          return;
        }
      }
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;
  }

  public override void ExitState(PlayerStateMachine stateM, PlayerManager player)
  {
    _startedJumpTouchingWall = false;
  }
}
