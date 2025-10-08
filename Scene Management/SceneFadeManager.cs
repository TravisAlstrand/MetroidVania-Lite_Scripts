using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SceneFadeManager : MonoBehaviour
{
  [SerializeField] private float _fadeDuration = 1f;
  private CanvasGroup _canvasGroup;

  public bool IsFadingOutOfScene { get; private set; }
  public bool IsFadingIntoScene { get; private set; }

  public static SceneFadeManager Instance;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    _canvasGroup = GetComponent<CanvasGroup>();
  }

  public void StartFadeOutOfScene()
  {
    IsFadingOutOfScene = true;
    StartCoroutine(FadeToBlack());
  }

  public void StartFadeIntoScene()
  {
    StartCoroutine(FadeToTransparent());
  }

  private IEnumerator FadeToTransparent()
  {
    PlayerManager.Instance.PlayerInput.EnablePlayerControls();
    float timer = 0f;
    float startAlpha = _canvasGroup.alpha;
    float endAlpha = 0f;

    while (timer < _fadeDuration)
    {
      timer += Time.deltaTime;
      _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / _fadeDuration);
      yield return null;
    }
    _canvasGroup.alpha = 0f;
  }

  private IEnumerator FadeToBlack()
  {
    float timer = 0f;
    float startAlpha = _canvasGroup.alpha;
    float endAlpha = 1f;

    while (timer < _fadeDuration)
    {
      timer += Time.deltaTime;
      _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / _fadeDuration);
      yield return null;
    }
    _canvasGroup.alpha = 1f;
    IsFadingOutOfScene = false;
  }
}
