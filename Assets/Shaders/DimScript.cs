using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimScript : MonoBehaviour
{
    [Range(0, 1)]
    public float dimValue = 1;

    public void MoveDimValue(float startValue, float targetValue, float time)
    {
        StartCoroutine(MoveDimValueCoroutine(startValue, targetValue, time));
    }

    // Canvas Version
    public RawImage dimCanvas;

    private void Awake()
    {
        if (dimCanvas == null)
        {
            dimCanvas = GameObject.FindObjectOfType<RawImage>();
        }
    }

    private IEnumerator MoveDimValueCoroutine(float startValue, float targetValue, float time)
    {
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            dimValue = Mathf.Lerp(startValue, targetValue, timeElapsed / time);
            if (dimCanvas != null)
            {
                dimCanvas.color = Color.Lerp(Color.black, Color.clear, dimValue);
            }
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }

    // Shader version
    /*
        private Material dimMaterial;
        private void Awake()
        {
            dimMaterial = new Material(Shader.Find("Custom/DimShader"));
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, dimMaterial);
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
    */

}
