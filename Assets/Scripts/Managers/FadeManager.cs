using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void FadeOutToIn(System.Action onMidFade = null)
    {
        StartCoroutine(FadeRoutine(onMidFade));
    }

    private IEnumerator FadeRoutine(System.Action onMidFade)
    {
        // Fade Out (ekraný karart)
        yield return StartCoroutine(Fade(0, 1));
        onMidFade?.Invoke(); // Yükleme burada yapýlýr (örnek: sahne deðiþimi)
        yield return new WaitForSeconds(0.2f);
        // Fade In (ekraný tekrar aç)
        yield return StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, time / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, to);
    }
}
