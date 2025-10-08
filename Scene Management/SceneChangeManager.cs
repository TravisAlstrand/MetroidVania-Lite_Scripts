using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
  public static SceneChangeManager Instance;
  [SerializeField] private CinemachineCamera _followCam;
  private SceneTrigger.DoorLabel _doorLabelToSpawnTo;
  private Vector3 _playerSpawnPosition;
  private bool _loadFromDoor = false;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  public void LoadNextSceneFromDoor(SceneField sceneToLoad, SceneTrigger.DoorLabel doorLabelToSpawnTo)
  {
    _loadFromDoor = true;
    StartCoroutine(FadeOutThenLoadScene(sceneToLoad, doorLabelToSpawnTo));
  }

  private IEnumerator FadeOutThenLoadScene(SceneField sceneToLoad, SceneTrigger.DoorLabel doorLabelToSpawnTo)
  {
    PlayerManager.Instance.PlayerInput.DisablePlayerControls();
    SceneFadeManager.Instance.StartFadeOutOfScene();

    while (SceneFadeManager.Instance.IsFadingOutOfScene)
    {
      yield return null;
    }

    _doorLabelToSpawnTo = doorLabelToSpawnTo;
    SceneManager.LoadScene(sceneToLoad);
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (_loadFromDoor)
    {
      FindDoor(_doorLabelToSpawnTo);
      PlayerManager.Instance.transform.position = _playerSpawnPosition;
      StartCoroutine(SnapCamera());
      _loadFromDoor = false;
    }
    SceneFadeManager.Instance.StartFadeIntoScene();
  }

  private void FindDoor(SceneTrigger.DoorLabel doorLabel)
  {
    SceneTrigger[] doors = FindObjectsByType<SceneTrigger>(FindObjectsSortMode.None);

    foreach (SceneTrigger door in doors)
    {
      if (door.CurrentDoorLabel == doorLabel)
      {
        _playerSpawnPosition = door.SpawnPoint.position;
        return;
      }
    }
  }

  private IEnumerator SnapCamera()
  {
    var transposer = _followCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;
    if (transposer != null)
    {
      Vector3 originalDamping = transposer.Damping;
      transposer.Damping = Vector3.zero;

      yield return null; // wait one frame so it snaps instantly

      transposer.Damping = originalDamping;
    }
  }
}
