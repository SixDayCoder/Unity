using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetection : PostEffectBase {

    public Shader EdgeDetectionShader;

    private Material EdgeDetectionMaterial = null;
    public Material material {
        get {
            EdgeDetectionMaterial = CheckShaderAndCreateMaterial(EdgeDetectionShader, EdgeDetectionMaterial);
            return EdgeDetectionMaterial;
        }
    }

    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;//是否仅描边

    public Color edgeColor = Color.black;//边缘颜色

    public Color bgColor = Color.white;//背景颜色

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if(material != null) {
            material.SetFloat("_EdgeOnly", edgesOnly);
            material.SetColor("_EdgeColor", edgeColor);
            material.SetColor("_BgColor", bgColor);

            Graphics.Blit(source, destination, material);
        }
        else {
            Graphics.Blit(source, destination);
        }
    }
}
