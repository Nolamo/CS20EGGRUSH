using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeMultiplier;

    [HideInInspector]
    public bool GameEnded = true;

    public IEnumerator Shake(float duration, float magnitude)
    {
        if(!GameEnded)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude * shakeMultiplier;
                float y = Random.Range(-1f, 1f) * magnitude * shakeMultiplier;

                transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = originalPos;
        }
    }
}
