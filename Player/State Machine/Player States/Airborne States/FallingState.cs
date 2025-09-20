using UnityEngine;

public class FallingState : PlayerBaseState
{
  private readonly string _animationName = "Jumping";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.IsGrounded)
    {
      stateM.SwitchState(stateM._landingState);
      return;
    }

    if (player.CanPerformJump() || player.CanPerformWallJump())
    {
      stateM.SwitchState(stateM._jumpingState);
      return;
    }

    if (player.IsOnWall && !player.IsGrounded)
    {
      stateM.SwitchState(stateM._wallSlidingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    // ADD EXTRA GRAVITY
    player.Rigidbody.linearVelocityY -= player.ExtraGravity * Time.fixedDeltaTime;

    // CLAMP FALL SPEED
    player.Rigidbody.linearVelocityY = Mathf.Max(player.Rigidbody.linearVelocityY, -player.MaxFallSpeed);

    // HORIZONTAL MOVEMENT
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;
  }
}
