using UnityEngine;

public class ExitWaterJumpState : PlayerBaseState
{
  private readonly string _animationName = "Jumping";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);
    player.Rigidbody.linearVelocityY = 0f;
    player.Rigidbody.gravityScale = player.NormalGravity;
    player.Rigidbody.AddForceY(player.ExitWaterJumpForce, ForceMode2D.Impulse);
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (player.Rigidbody.linearVelocityY <= 0f)
    {
      stateM.SwitchState(stateM._fallingState);
      return;
    }

    if (player.CanAttack)
    {
      stateM.SwitchState(stateM._attackingState);
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
      return;
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

    // WALL SLIDE
    if (player.IsOnWall && player.WallAbilitiesUnlocked)
    {
      if (!player.IsGrounded)
      {
        stateM.SwitchState(stateM._wallSlidingState);
        return;
      }
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;
  }
}
