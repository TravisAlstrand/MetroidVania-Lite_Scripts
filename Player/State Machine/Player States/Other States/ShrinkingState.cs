using UnityEngine;

public class ShrinkingState : PlayerBaseState
{
  private readonly string _animationName = "Shrinking";
  private float _animationLength;
  private float _stateTimer;

  public override void EnterState(PlayerStateMachine stateM, PlayerManager player)
  {
    // CHANGE COLOR / PLAY ANIMATION
    player.ChangeSpriteColor(player.ShrinkColor);
    player.Animator.Play(_animationName);
    // GET ANIMATION LENGTH TO AUTO SWITCH STATES WHEN COMPLETED
    AnimationClip clip = player.GetClipByName(_animationName);
    _animationLength = clip.length;
    _stateTimer = _animationLength;

    player.SwitchBodyColliders();
    player.IsSmall = true;
  }

  public override void UpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    _stateTimer -= Time.deltaTime;

    if (_stateTimer <= 0f)
    {
      if (player.FrameInput.Move.x != 0f)
      {
        stateM.SwitchState(stateM._smallMovingState);
        return;
      }
      else
      {
        stateM.SwitchState(stateM._smallIdleState);
      }
    }
  }

  public override void FixedUpdateState(PlayerStateMachine stateM, PlayerManager player)
  {
    player.Rigidbody.linearVelocityX = player.FrameInput.Move.x * player.MoveSpeed;
  }
}
