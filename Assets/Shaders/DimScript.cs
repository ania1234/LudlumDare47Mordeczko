using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimScript : MonoBehaviour
{
    // Canvas Version
    public RawImage dimCanvas;

    private void Awake()
    {
        if (dimCanvas == null)
        {
            dimCanvas = GameObject.FindObjectOfType<RawImage>();
        }
    }

    public void SetDimValue(float dimValue)
    {
        if (dimCanvas != null)
        {
            dimCanvas.color = Color.Lerp(Color.black, Color.clear, dimValue);
        }
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
