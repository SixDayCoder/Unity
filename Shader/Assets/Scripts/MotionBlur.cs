using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : PostEffectBase{

    public Shader MotionBlurShader;
    private Material MotionBlurMaterial = null;
    public Material material {
        get {
            MotionBlurMaterial = CheckShaderAndCreateMaterial(MotionBlurShader, MotionBlurMaterial);
            return MotionBlurMaterial;
        }
    }

    [Range(0.0f, 0.9f)]
    public float BlurAmount = 0.5f;
    //BlurAmount的值越大,拖尾效果越明显

    private RenderTexture AccumulationBuffer = null;

    void CreateAccumulationBuffer(int width, int height) {
        AccumulationBuffer = new RenderTexture(width, height, 0);
        AccumulationBuffer.hideFlags = HideFlags.HideAndDontSave;//因为我们要自己控制该变量的生成与销毁,不让它出现在Hierarchy面板上,也不会保存到场景
    }

    void OnDisable() {
        DestroyImmediate(AccumulationBuffer);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material != null) {
            if (AccumulationBuffer        == null         ||
                AccumulationBuffer.width  != source.width ||
                AccumulationBuffer.height != source.height ) {

                DestroyImmediate(AccumulationBuffer);
                CreateAccumulationBuffer(source.width, source.height);
                Graphics.Blit(source, AccumulationBuffer);
                //现在已经有了可用的累积纹理缓存

                AccumulationBuffer.MarkRestoreExpected();//????????

                material.SetFloat("_BlurAmount", 1.0f - BlurAmount);

                Graphics.Blit(source, AccumulationBuffer, material);
                Graphics.Blit(AccumulationBuffer, destination);   

            }
        }
        else {
            Graphics.Blit(source, destination);
        }
    }


}
