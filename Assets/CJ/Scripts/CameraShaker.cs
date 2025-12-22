using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }
    Vector3 originalPos;

    void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float amplitude, float duration)
    {
        StartCoroutine(Co_Shake(amplitude, duration));
    }

    IEnumerator Co_Shake(float amp, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float x = Random.Range(-1f, 1f) * amp;
            float y = Random.Range(-1f, 1f) * amp;
            transform.localPosition = originalPos + new Vector3(x, y, 0);
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}