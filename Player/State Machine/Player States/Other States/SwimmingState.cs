using UnityEngine;

public class SwimmingState : PlayerBaseState
{
  private readonly string _animationName = "Swimming";

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.ChangeSpriteColor(player.WaterColor);
    player.Animator.Play(_animationName);
    player.Rigidbody.linearVelocity = new Vector2(0f, 1f);
    player.Rigidbody.gravityScale = player.WaterGravity;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    if (!player.IsUnderWater && player.Rigidbody.linearVelocityY > 0f)
    {
      stateM.SwitchState(stateM._jumpingState);
      return;
    }

    if (!player.IsUnderWater && player.Rigidbody.linearVelocityY <= 0f)
    {
      stateM.SwitchState(stateM._fallingState);
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.SwimMoveForce;
  }
}
