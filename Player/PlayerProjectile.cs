using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
  [SerializeField] private float _projectileForce;
  [SerializeField] private int _damage = 10;
  [SerializeField] private GameObject _impactParticles;
  [SerializeField] private float _maxLifeTime = 3f;
  private float _projectileDirection;
  private float _lifeTimer;

  private Rigidbody2D _rigidbody;

  private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  private void Start()
  {
    _projectileDirection = PlayerManager.Instance.IsFacingRight ? 1 : -1;
    _lifeTimer = _maxLifeTime;
  }

  private void Update()
  {
    _lifeTimer -= Time.deltaTime;

    if (_lifeTimer <= 0f)
    {
      Destroy(gameObject);
    }
  }

  private void FixedUpdate()
  {
    _rigidbody.linearVelocityX = _projectileDirection * _projectileForce;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
    {
      damageable.TakeDamage(_damage);
    }
    Destroy(gameObject);
  }
}
