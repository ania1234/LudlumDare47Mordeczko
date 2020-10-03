using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimScript : MonoBehaviour
{
    public Material dimMaterial;
    [Range(0, 1)]
    public float dimValue = 1;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, dimMaterial);
    }

    public void MoveDimValue(float startValue, float targetValue, float time)
    {
        StartCoroutine(MoveDimValueCoroutine(startValue, targetValue, time));
    }

    private IEnumerator MoveDimValueCoroutine(float startValue, float targetValue, float time)
    {
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            dimValue = Mathf.Lerp(startValue, targetValue, timeElapsed / time);
            dimMaterial.SetFloat("_DimFactor", dimValue);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }
}
