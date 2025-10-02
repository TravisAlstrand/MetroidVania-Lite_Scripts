using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
  public enum DoorLabel
  {
    None,
    A,
    B,
    C,
    D
  }

  [Header("To Load Into:")]
  [SerializeField] private SceneField _sceneToLoad;
  [SerializeField] private DoorLabel _doorLabelToSpawnTo;

  [Header("Current:")]
  [SerializeField] private DoorLabel _thisDoorLabel;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      SceneChangeManager.Instance.LoadNextSceneFromDoor(_sceneToLoad, _doorLabelToSpawnTo);
    }
  }
}
