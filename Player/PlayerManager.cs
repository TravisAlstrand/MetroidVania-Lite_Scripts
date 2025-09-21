using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerManager : MonoBehaviour
{
  [Header("Ground")]
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private Transform _groundHitRayPointLeft;
  [SerializeField] private Transform _groundHitRayPointRight;
  [SerializeField] private float _groundDetectionRayLength = 0.2f;
  [SerializeField] private bool _showGroundHitRays = false;
  private RaycastHit2D _groundHitInfoLeft;
  private RaycastHit2D _groundHitInfoRight;
  private bool _isGrounded;

  [Header("Wall")]
  [SerializeField] private LayerMask _wallLayer;
  [SerializeField] private Transform _wallHitRayPointUpper;
  [SerializeField] private Transform _wallHitRayPointLower;
  [SerializeField] private float _wallDetectionRayLength = 0.2f;
  [SerializeField] private bool _showWallHitRays = false;
  private RaycastHit2D _wallHitInfoUpper;
  private RaycastHit2D _wallHitInfoLower;
  private bool _isOnWall;

  [Header("Movement")]
  public float MoveSpeed = 6f;
  // MAY NEED TO CHANGE THIS DEFAULT VALUE EVENTUALLY
  private bool _isFacingRight = true;

  [Header("Jumping")]
  public float JumpForce = 11.25f;
  [SerializeField] private float _coyoteTime = 0.13f;
  [SerializeField] private float _jumpBufferTime = 0.2f;
  [SerializeField] private float _jumpGroundIgnoreTime = 0.1f;
  public float IgnoreWallOnJumpTime = 0.2f;
  private float _jumpBufferTimer;
  private float _coyoteTimer;
  [HideInInspector] public float _lastJumpTime;
  private bool _jumpQueued = false;

  [Header("Falling")]
  public float ExtraGravity = 25f;
  public float MaxFallSpeed = 25f;

  [Header("Wall Jump")]
  public Vector2 WallJumpForce;
  public float MinWallJumpTime;
  public float MaxWallJumpTime;
  [SerializeField] private float _wallCoyoteTime = 0.12f;
  private float _wallCoyoteTimer;

  [Header("Wall Slide")]
  public float WallSlideSpeed;

  [Header("Dash")]
  public float DashSpeed = 12f;
  public float DashDuration = 1f;
  [SerializeField] private float _dashCoolDown = 1f;
  private float _dashCoolDownTimer;
  private bool _shouldCountdownDashCoolDown = false;

  [Header("Shrink/Grow")]
  public float SmallMoveSpeed = 4.25f;
  [SerializeField] private Transform _smallHeadHitRayPointLeft;
  [SerializeField] private Transform _smallHeadHitRayPointRight;
  [SerializeField] private float _smallHeadDetectionRayLength = 0.3f;
  [SerializeField] private bool _showSmallHeadHitRays = false;
  private RaycastHit2D _smallHeadHitInfoLeft;
  private RaycastHit2D _smallHeadHitInfoRight;
  private bool _isUnderLowCeiling = false;
  [HideInInspector] public bool IsSmall = false;

  [Header("Ability Unlocks")]
  [SerializeField] private bool _wallAbilitiesUnlocked = false;
  [SerializeField] private bool _doubleJumpUnlocked = false;
  [SerializeField] private bool _dashUnlocked = false;
  [SerializeField] private bool _shrinkUnlocked = false;
  private bool _canDoubleJump = false;

  [Header("Ability Colors")]
  public SpriteRenderer FillSpriteRenderer;
  public Color WallColor;
  public Color DashColor;
  public Color ShrinkColor;

  // COMPONENTS
  [HideInInspector] public Rigidbody2D Rigidbody;
  [HideInInspector] public Animator Animator;
  private PlayerInputManager _playerInput;
  [HideInInspector] public FrameInput FrameInput;
  private Collider2D _tallBodyCollider;
  private Collider2D _smallBodyCollider;
  private PlayerStateMachine _stateM;


  // GETTERS
  public bool IsFacingRight => _isFacingRight;
  public bool IsGrounded
  {
    get
    {
      if (Time.time < _lastJumpTime + _jumpGroundIgnoreTime)
      {
        // IGNORE GROUND IMMEDIATELY AFTER JUMP
        // TO PREVENT RUINED JUMP AND LANDING STATE GLITCH
        return false;
      }
      return _isGrounded;
    }
  }
  public bool IsOnWall => _isOnWall;
  public bool WallAbilitiesUnlocked => _wallAbilitiesUnlocked;
  public bool CanDash => DetermineIfCanDash();
  public bool CanShrink => DetermineIfCanShrink();
  public bool CanGrow => DetermineIfCanGrow();

  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody2D>();
    Animator = GetComponent<Animator>();
    _playerInput = GetComponent<PlayerInputManager>();
    _tallBodyCollider = GetComponent<CapsuleCollider2D>();
    _smallBodyCollider = GetComponent<CircleCollider2D>();
    _stateM = GetComponent<PlayerStateMachine>();
  }

  private void Update()
  {
    GatherInput();
    CountTimers();
    HandleJumpInput();
    FlipSprite();
  }

  private void FixedUpdate()
  {
    CollisionChecks();
  }

  private void GatherInput()
  {
    FrameInput = _playerInput.FrameInput;
  }

  private void CountTimers()
  {
    // JUMP BUFFER
    _jumpBufferTimer -= Time.deltaTime;

    // COYOTE TIME
    if (!_isGrounded)
    {
      _coyoteTimer -= Time.deltaTime;
    }
    else
    {
      _coyoteTimer = _coyoteTime;
      _canDoubleJump = true;
    }

    // WALL COYOTE TIME (WHEN PUSHING OFF WALL AND ATTEMPTING WALL JUMP LATE)
    if (!_isOnWall)
    {
      _wallCoyoteTimer -= Time.deltaTime;
    }
    else
    {
      _wallCoyoteTimer = _wallCoyoteTime;
    }

    if (_shouldCountdownDashCoolDown)
    {
      _dashCoolDownTimer -= Time.deltaTime;
    }
  }

  #region Sprite Flip
  private void FlipSprite()
  {
    if ((_isFacingRight && FrameInput.Move.x < 0f) ||
        !_isFacingRight && FrameInput.Move.x > 0f)
    {
      transform.Rotate(0f, 180f, 0f);
      _isFacingRight = !_isFacingRight;
    }
  }

  public void ForceFlipSprite()
  {
    transform.Rotate(0f, 180f, 0f);
    _isFacingRight = !_isFacingRight;
  }
  #endregion

  #region Jumping
  private void HandleJumpInput()
  {
    if (FrameInput.Jump)
    {
      _jumpQueued = true;
      _jumpBufferTimer = _jumpBufferTime;
    }
  }

  public bool CanPerformJump()
  {
    // BUFFER CHECK
    if (!_jumpQueued || _jumpBufferTimer <= 0f || IsSmall) return false;

    // PRIMARY JUMP (GROUNDED OR COYOTE)
    if (_coyoteTimer > 0f)
    {
      _jumpQueued = false;
      return true;
    }

    // DOUBLE JUMP
    if (_doubleJumpUnlocked && _canDoubleJump)
    {
      _jumpQueued = false;
      _canDoubleJump = false;
      return true;
    }

    return false;
  }
  #endregion

  #region Wall Jumping
  public bool CanPerformWallJump()
  {
    // MUST HAVE ABILITY UNLOCKED
    if (!_wallAbilitiesUnlocked) return false;

    // MUST HAVE JUMP QUEUED
    if (!_jumpQueued) return false;

    // WALL COYOTE CHECK
    if (_wallCoyoteTimer > 0f)
    {
      _jumpQueued = false;
      return true;
    }

    return false;
  }
  #endregion

  #region Dashing
  public void StartDashCoolDown()
  {
    _dashCoolDownTimer = _dashCoolDown;
    _shouldCountdownDashCoolDown = true;
  }

  private bool DetermineIfCanDash()
  {
    if (!FrameInput.Dash) return false;

    if (_dashUnlocked && _dashCoolDownTimer <= 0f && !_isOnWall)
    {
      _shouldCountdownDashCoolDown = false;
      return true;
    }
    return false;
  }
  #endregion

  #region Shrinking / Growing
  public void SwitchBodyColliders()
  {
    _tallBodyCollider.enabled = !_tallBodyCollider.enabled;
    _smallBodyCollider.enabled = !_smallBodyCollider.enabled;
  }

  private bool DetermineIfCanShrink()
  {
    if (!FrameInput.ShrinkGrow || IsSmall) return false;

    if (_shrinkUnlocked && _isGrounded && _stateM.CurrentState != _stateM._dashingState)
    {
      return true;
    }
    return false;
  }

  private bool DetermineIfCanGrow()
  {
    if (!FrameInput.ShrinkGrow || !IsSmall) return false;

    if (_shrinkUnlocked && _isGrounded && !_isUnderLowCeiling)
    {
      return true;
    }
    return false;
  }
  #endregion

  #region Helpers
  public AnimationClip GetClipByName(string name)
  {
    AnimationClip[] clips = Animator.runtimeAnimatorController.animationClips;

    foreach (AnimationClip clip in clips)
    {
      if (clip.name == name) return clip;
    }
    return null;
  }
  #endregion

  #region Collision Checks

  private void CheckIfGrounded()
  {
    _groundHitInfoLeft = Physics2D.Raycast(_groundHitRayPointLeft.position, Vector2.down, _groundDetectionRayLength, _groundLayer);
    _groundHitInfoRight = Physics2D.Raycast(_groundHitRayPointRight.position, Vector2.down, _groundDetectionRayLength, _groundLayer);

    if (_showGroundHitRays)
    {
      Debug.DrawRay(_groundHitRayPointLeft.position, new Vector3(0f, -_groundDetectionRayLength, 0f), Color.red);
      Debug.DrawRay(_groundHitRayPointRight.position, new Vector3(0f, -_groundDetectionRayLength, 0f), Color.red);
    }

    if (_groundHitInfoLeft || _groundHitInfoRight)
    {
      _isGrounded = true;
    }
    else { _isGrounded = false; }
  }

  private void CheckIfOnWall()
  {
    int rayDirection;

    if (_isFacingRight) { rayDirection = 1; }
    else { rayDirection = -1; }

    _wallHitInfoUpper = Physics2D.Raycast(_wallHitRayPointUpper.position, new Vector2(rayDirection, 0f), _wallDetectionRayLength, _wallLayer);
    _wallHitInfoLower = Physics2D.Raycast(_wallHitRayPointLower.position, new Vector2(rayDirection, 0f), _wallDetectionRayLength, _wallLayer);

    if (_showWallHitRays)
    {
      Debug.DrawRay(_wallHitRayPointUpper.position, new Vector3(_wallDetectionRayLength, 0f, 0f), Color.red);
      Debug.DrawRay(_wallHitRayPointLower.position, new Vector3(_wallDetectionRayLength, 0f, 0f), Color.red);
    }

    if (_wallHitInfoUpper || _wallHitInfoLower)
    {
      _isOnWall = true;
    }
    else { _isOnWall = false; }
  }

  private void CheckIfUnderLowCeiling()
  {
    _smallHeadHitInfoLeft = Physics2D.Raycast(_smallHeadHitRayPointLeft.position, Vector2.up, _smallHeadDetectionRayLength, _groundLayer);
    _smallHeadHitInfoRight = Physics2D.Raycast(_smallHeadHitRayPointRight.position, Vector2.up, _smallHeadDetectionRayLength, _groundLayer);

    if (_showSmallHeadHitRays)
    {
      Debug.DrawRay(_smallHeadHitRayPointLeft.position, new Vector3(0f, _smallHeadDetectionRayLength, 0f), Color.red);
      Debug.DrawRay(_smallHeadHitRayPointRight.position, new Vector3(0f, _smallHeadDetectionRayLength, 0f), Color.red);
    }

    if (_smallHeadHitInfoLeft || _smallHeadHitInfoRight)
    {
      _isUnderLowCeiling = true;
    }
    else { _isUnderLowCeiling = false; }
  }

  private void CollisionChecks()
  {
    CheckIfGrounded();
    CheckIfOnWall();
    if (IsSmall) CheckIfUnderLowCeiling();
  }
  #endregion
}

