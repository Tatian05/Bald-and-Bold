using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour
{
    public GameObject camera;
    public float duration;
    public float magnitud;
    
    public void Shake()
    {
        StartCoroutine(ShakeShake());
    }
     IEnumerator ShakeShake()
    {
        Vector3 originalPos = camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * magnitud;
            float y = Random.Range(-1f, 1f) * magnitud;

            camera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        camera.transform.localPosition = originalPos;
    }  
    
}
