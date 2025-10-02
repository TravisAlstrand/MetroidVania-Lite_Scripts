using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
  public static SceneChangeManager Instance;
  private SceneTrigger.DoorLabel _doorToSpawnTo;
  private bool _loadFromDoor;

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

  public void LoadNextSceneFromDoor(SceneField sceneToLoad, SceneTrigger.DoorLabel doorToSpawnTo = SceneTrigger.DoorLabel.None)
  {
    _loadFromDoor = true;
    StartCoroutine(FadeOutThenLoadScene(sceneToLoad, doorToSpawnTo));
  }

  private IEnumerator FadeOutThenLoadScene(SceneField sceneToLoad, SceneTrigger.DoorLabel doorToSpawnTo)
  {
    SceneFadeManager.Instance.StartFadeOut();

    while (SceneFadeManager.Instance.IsFadingOut)
    {
      yield return null;
    }

    _doorToSpawnTo = doorToSpawnTo;
    SceneManager.LoadScene(sceneToLoad);
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log("OnSceneLoadedCalled");
    SceneFadeManager.Instance.StartFadeIn();

    if (_loadFromDoor)
    {
      // 
      _loadFromDoor = false;
    }
  }
}
