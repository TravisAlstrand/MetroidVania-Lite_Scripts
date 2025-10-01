using UnityEngine;

public class SmallFallingState : PlayerBaseState
{
  private readonly string _animationName = "SmallFalling";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.IsGrounded)
    {
      if (player.FrameInput.Move.x != 0f)
      {
        stateM.SwitchState(stateM._smallMovingState);
        return;
      }
      stateM.SwitchState(stateM._smallIdleState);
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

    if (player.IsOnWaterCannotSwim)
    {
      player.Rigidbody.linearVelocityY = 0f;
      player.Rigidbody.AddForceY(player.WaterBumpCannotSwim);
      player.IsOnWaterCannotSwim = false;
    }
  }
}
