using UnityEngine;

public interface IKnockBackable
{
  public void ApplyKnockBack(Vector2 direction, float force, float duration);
}