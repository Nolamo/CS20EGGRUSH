using System.Collections;
using UnityEngine;

/// <summary> Shakes the screen, yo </summary>
public class ScreenShake : MonoBehaviour
{
    /// <summary> Intensity of the shake. </summary>
    public float shakeMultiplier;

    [HideInInspector]
    public bool GameEnded = true; // To prevent mass screen shakeage when the game ends on rare occasions 

    public IEnumerator Shake(float duration, float magnitude)
    {
        if(!GameEnded)
        {
            Vector3 originalPos = transform.localPosition; // store original position

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude * shakeMultiplier;
                float y = Random.Range(-1f, 1f) * magnitude * shakeMultiplier;

                transform.localPosition = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = originalPos; // reset camera position
        }
    }
}
