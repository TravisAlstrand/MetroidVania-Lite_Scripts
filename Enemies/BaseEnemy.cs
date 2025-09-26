using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class EnemyBase : MonoBehaviour, IKnockBackable
{
  protected Rigidbody2D _rigidbody;
  protected Health _health;
  protected bool _isStunned;

  protected virtual void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
    _health = GetComponent<Health>();

    // SUBSCRIBE TO HEALTH'S ONDEATH ACTION
    _health.OnDeath += HandleDeath;
  }

  public virtual void ApplyKnockBack(Vector2 direction, float force, float duration)
  {
    if (_isStunned) return;

    _rigidbody.linearVelocity = Vector2.zero;
    _rigidbody.AddForce(direction.normalized * force, ForceMode2D.Impulse);

    StartCoroutine(StunCoroutine(duration));
  }

  protected virtual IEnumerator StunCoroutine(float duration)
  {
    _isStunned = true;
    // TODO: DISABLE AI / MOVEMENT
    yield return new WaitForSeconds(duration);
    _isStunned = false;
    // TODO: RE-ENABLE AI / MOVEMENT
  }

  protected virtual void HandleDeath()
  {
    Destroy(gameObject);
  }
}
