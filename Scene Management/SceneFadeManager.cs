using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
  [SerializeField] private Image _fadeImage;
  [SerializeField] private float _fadeSpeed = 5f;
  [SerializeField] private Color _fadeStartColor;

  public bool IsFadingOut { get; private set; }
  public bool IsFadingIn { get; private set; }

  public static SceneFadeManager Instance;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }

    _fadeStartColor.a = 0f;
  }

  private void Update()
  {
    if (IsFadingOut)
    {
      if (_fadeStartColor.a < 1f)
      {
        _fadeStartColor.a += Time.deltaTime * _fadeSpeed;
        _fadeImage.color = _fadeStartColor;
      }
      else { IsFadingOut = false; }
    }

    if (IsFadingIn)
    {
      if (_fadeStartColor.a > 0f)
      {
        _fadeStartColor.a -= Time.deltaTime * _fadeSpeed;
        _fadeImage.color = _fadeStartColor;
      }
      else { IsFadingIn = false; }
    }
  }

  public void StartFadeOut()
  {
    _fadeImage.color = _fadeStartColor;
    IsFadingOut = true;
  }

  public void StartFadeIn()
  {
    Debug.Log("Fade in called");
    if (_fadeImage.color.a >= 1f)
    {
      _fadeImage.color = _fadeStartColor;
      IsFadingIn = true;
    }
  }
}
