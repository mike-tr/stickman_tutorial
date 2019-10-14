using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class OnPostRender : MonoBehaviour
{

    public Shader shader;
    public Material material;

    [Range(0, 0.01f)]
    public float _offset = 0.002f;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!material)
        {
            material = new Material(shader);
        }
        Debug.Log((float)source.height / source.width + " " + source.width + " " + source.height);
        material.SetFloat("_ratio", (float)source.height / source.width);
        material.SetFloat("_offset", _offset);
        Graphics.Blit(source, destination, material);
    }
}
