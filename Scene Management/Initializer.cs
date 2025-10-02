using UnityEngine;

public static class Initializer
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void Execute()
  {
    Debug.Log("Loaded Persist Object by Initializer script");
    Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("--PersistentObjects--")));
  }
}
