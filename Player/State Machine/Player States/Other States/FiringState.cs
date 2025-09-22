using UnityEngine;

public class FiringState : PlayerBaseState
{
  private readonly string _animationName = "Fire";
  private float _animationLength;
  private float _stateTimer;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.ChangeSpriteColor(player.FireColor);
    player.Animator.Play(_animationName);
    player.FireProjectile();
    // GET ANIMATION LENGTH TO AUTO SWITCH STATES WHEN COMPLETED
    AnimationClip clip = player.GetClipByName(_animationName);
    _animationLength = clip.length;
    _stateTimer = _animationLength;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _stateTimer -= Time.deltaTime;

    if (_stateTimer <= 0f)
    {
      if (player.IsGrounded)
      {
        if (player.FrameInput.Move.x != 0f)
        {
          stateM.SwitchState(stateM._movingState);
          return;
        }
        stateM.SwitchState(stateM._idleState);
        return;
      }
      else
      {
        if (player.Rigidbody.linearVelocityY > 0f)
        {
          stateM.SwitchState(stateM._jumpingState);
          return;
        }
        stateM.SwitchState(stateM._fallingState);
      }
    }
  }

  public override void ExitState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.StartProjectileCountDown();
  }
}
