using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
  [SerializeField] private int _maxHealth = 10;
  private int _currentHealth;

  public System.Action OnDeath;

  private void Start()
  {
    _currentHealth = _maxHealth;
  }

  public void TakeDamage(int amount)
  {
    _currentHealth -= amount;
    if (_currentHealth <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    OnDeath?.Invoke();
  }
}