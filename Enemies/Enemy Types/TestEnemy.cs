public class TestEnemy : EnemyBase
{
  protected override void Awake()
  {
    base.Awake();
    // Enemy specific setup here
  }

  private void Update()
  {
    if (_isStunned) return;

    // AI attack logic here
  }

  private void FixedUpdate()
  {
    if (_isStunned) return;
    _rigidbody.linearVelocityX = -2f;
  }
}