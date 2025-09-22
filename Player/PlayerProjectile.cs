using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
  [SerializeField] private float _projectileForce;
  [SerializeField] private int _damage;
  [SerializeField] private GameObject impactParticles;
  private float _projectileDirection;

  private Rigidbody2D _rigidbody;

  private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  private void Start()
  {
    _projectileDirection = PlayerManager.Instance.IsFacingRight ? 1 : -1;
  }

  private void FixedUpdate()
  {
    _rigidbody.linearVelocityX = _projectileDirection * _projectileForce;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    Destroy(gameObject);
  }
}
