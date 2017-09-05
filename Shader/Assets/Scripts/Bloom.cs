using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : PostEffectBase {

    public Shader BloomShader;
    private Material BloomMaterial;

    public Material material {
        get {
            BloomMaterial = CheckShaderAndCreateMaterial(BloomShader, BloomMaterial);
            return BloomMaterial;
        }
    }

    [Range(0, 4)]
    public int Iterations = 3;

    [Range(0.2f, 3.0f)]
    public float BlurSpread = 0.6f;

    [Range(1, 8)]
    public int DownSample = 2;

    [Range(0.0f, 4.0f)]
    public float LuminanceThreshold = 0.6f;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if(material != null) {
            material.SetFloat("_LuminanceThreshold", LuminanceThreshold);

            int width = source.width / DownSample;
            int height = source.height / DownSample;
            RenderTexture currBuffer = RenderTexture.GetTemporary(width, height);//只对第一个buffer降采样
            currBuffer.filterMode = FilterMode.Bilinear;

            Graphics.Blit(source, currBuffer, material, 0); //提取出了光亮纹理

            for (int i = 0; i < Iterations; ++i) {

                material.SetFloat("_BlurSize", 1.0f + i * BlurSpread);

                RenderTexture nextBuffer = RenderTexture.GetTemporary(width, height);

                //render vertical
                Graphics.Blit(currBuffer, nextBuffer, material, 1);

                //指针后移
                RenderTexture.ReleaseTemporary(currBuffer);
                currBuffer = nextBuffer;
                nextBuffer = RenderTexture.GetTemporary(width, height);

                //render horizontal
                Graphics.Blit(currBuffer, nextBuffer, material, 2);

                RenderTexture.ReleaseTemporary(currBuffer);
                currBuffer = nextBuffer;

            }

            material.SetTexture("_Bloom", currBuffer);//已经高斯模糊后的buffer,接下来就是要和原图source混合
            Graphics.Blit(source, destination, material, 3);

        }
        else {
            Graphics.Blit(source, destination);
        }
    }

}
