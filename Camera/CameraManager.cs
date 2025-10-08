using UnityEngine;

public class CameraManager : MonoBehaviour
{
  [SerializeField] private GameObject _lookDownCamera;

  public bool LookDownCameraActive => _lookDownCamera.activeInHierarchy;

  private void Awake()
  {
    _lookDownCamera.SetActive(false);
  }

  public void ActivateLookDownCamera()
  {
    _lookDownCamera.SetActive(true);
  }

  public void DeactivateLookDownCamera()
  {
    _lookDownCamera.SetActive(false);
  }
}
