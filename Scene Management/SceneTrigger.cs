using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
  public enum DoorLabel
  {
    A,
    B,
    C,
    D
  }

  [Header("To Load Into:")]
  [SerializeField] private SceneField _sceneToLoad;
  [SerializeField] private DoorLabel _doorLabelToSpawnTo;

  [Header("Current:")]
  public DoorLabel CurrentDoorLabel;
  public Transform SpawnPoint;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      SceneChangeManager.Instance.LoadNextSceneFromDoor(_sceneToLoad, _doorLabelToSpawnTo);
    }
  }
}
