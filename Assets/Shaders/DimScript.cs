using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimScript : MonoBehaviour
{
    public Material dimMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, dimMaterial);
    }
}
