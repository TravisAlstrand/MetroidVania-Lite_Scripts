using UnityEngine;

public class AttackingState : PlayerBaseState
{
  private readonly string _animationName = "Attack1";
  private float _animationLength;
  private float _stateTimer;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Animator.Play(_animationName);

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
        else
        {
          stateM.SwitchState(stateM._idleState);
          return;
        }
      }
      else
      {
        if (player.WallAbilitiesUnlocked && player.IsOnWall)
        {
          stateM.SwitchState(stateM._wallSlidingState);
          return;
        }

        if (player.Rigidbody.linearVelocityY > 0f)
        {
          stateM.SwitchState(stateM._jumpingState);
          return;
        }
        else
        {
          stateM.SwitchState(stateM._fallingState);
        }
      }
    }
  }
}
