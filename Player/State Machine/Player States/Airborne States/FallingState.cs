using UnityEditorInternal;
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
    if (player.CanAttack)
    {
      stateM.SwitchState(stateM._attackingState);
      return;
    }

    if (player.CanDash)
    {
      stateM.SwitchState(stateM._dashingState);
    }

    if (player.CanShield)
    {
      stateM.SwitchState(stateM._shieldedState);
      return;
    }

    if (player.CanFire)
    {
      stateM.SwitchState(stateM._firingState);
      return;
    }

    if (player.CanJump || player.CanWallJump)
    {
      stateM.SwitchState(stateM._jumpingState);
      return;
    }

    if (player.IsOnWall && player.WallAbilitiesUnlocked && !player.IsGrounded)
    {
      stateM.SwitchState(stateM._wallSlidingState);
      return;
    }

    if (player.IsUnderWater && player.SwimAbilityUnlocked)
    {
      stateM.SwitchState(stateM._swimmingState);
    }

    if (player.IsGrounded)
    {
      if (!player.IsSmall)
      {
        stateM.SwitchState(stateM._landingState);
        return;
      }
      else
      {
        if (player.FrameInput.Move.x != 0f)
        {
          stateM.SwitchState(stateM._smallMovingState);
          return;
        }
        else
        {
          stateM.SwitchState(stateM._smallMovingState);
        }
      }
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
