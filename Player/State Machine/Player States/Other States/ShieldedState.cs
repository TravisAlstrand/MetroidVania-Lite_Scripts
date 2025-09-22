using UnityEngine;

public class ShieldedState : PlayerBaseState
{
  private readonly string _animationName = "Shielded";
  private float _enabledTimer;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.ChangeSpriteColor(player.WhiteColor);
    player.Rigidbody.linearVelocityX = 0f;
    player.SwitchGravityValues();
    player.Animator.Play(_animationName);
    _enabledTimer = player.ShieldDuration;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _enabledTimer -= Time.deltaTime;

    if (_enabledTimer <= 0f)
    {
      if (player.IsGrounded)
      {
        stateM.SwitchState(stateM._idleState);
        return;
      }

      stateM.SwitchState(stateM._fallingState);
    }
  }

  public override void ExitState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.StartShieldCoolDown();
    player.SwitchGravityValues();
    player.ChangeToPreviousSpriteColor();
  }
}
