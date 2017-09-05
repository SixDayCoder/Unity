using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBlur : PostEffectBase {

    public Shader GaussShader;

    private Material GaussMaterial = null;
    public Material material {
        get {
            GaussMaterial = CheckShaderAndCreateMaterial(GaussShader, GaussMaterial);
            return GaussMaterial;
        }
    }

    [Range(0, 4)]
    public int Iterations = 3;//高斯模糊迭代次数,次数越高越模糊,但是性能越差

    [Range(0.2f, 3.0f)]
    public float BlurSpread = 0.6f;//越大越模糊

    [Range(1, 8)]
    public int DownSample = 2;
   
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material != null) {

            /*
            RenderTexture buffer = RenderTexture.GetTemporary(source.width, source.height);//开辟临时缓冲区

            Graphics.Blit(source, buffer, material, 0);//使用第一个Pass渲染,垂着滤波
            Graphics.Blit(buffer, destination, material, 1);//使用第二个Pass渲染,水平滤波

            RenderTexture.ReleaseTemporary(buffer);//释放缓冲区

            这样做有性能问题,对图像就行降采样，减少要处理的像素的个数
            */
            /*
            int width = source.width / DownSample;
            int height = source.height / DownSample;
            RenderTexture buffer = RenderTexture.GetTemporary(width, height);//只对第一个buffer降采样
            buffer.filterMode = FilterMode.Bilinear;

            Graphics.Blit(source, buffer, material, 0);//使用第一个Pass渲染,垂着滤波
            Graphics.Blit(buffer, destination, material, 1);//使用第二个Pass渲染,水平滤波
            */
            /*完整版本,加入迭代次数的高斯模糊*/

            int width = source.width / DownSample;
            int height = source.height / DownSample;
            RenderTexture currBuffer = RenderTexture.GetTemporary(width, height);//只对第一个buffer降采样
            currBuffer.filterMode = FilterMode.Bilinear;

            Graphics.Blit(source, currBuffer, material);

            for(int i = 0; i < Iterations; ++i) {

                material.SetFloat("_BlurSize", 1.0f + i * BlurSpread);

                RenderTexture nextBuffer = RenderTexture.GetTemporary(width, height);
                
                //render vertical
                Graphics.Blit(currBuffer, nextBuffer, material, 0);

                //指针后移
                RenderTexture.ReleaseTemporary(currBuffer);
                currBuffer = nextBuffer;
                nextBuffer = RenderTexture.GetTemporary(width, height);

                //render horizontal
                Graphics.Blit(currBuffer, nextBuffer, material, 1);

                RenderTexture.ReleaseTemporary(currBuffer);
                currBuffer = nextBuffer;

            }

            Graphics.Blit(currBuffer, destination);


        }
        else {
            Graphics.Blit(source, destination);
        }
    }
}

/*
高斯模糊会使用一个 N x N的矩阵作为高斯核进行卷积操作, 对于一个W x H大的纹理图, 就需要进行 N x N x W x H 次采样
不高效,我们可以将二维的高斯函数拆分成两个一维函数, 简化到2 x N x W x H次操作
两个一维函数中有很多重复的值,可化简

我们调用两次Pass,一次使用竖直方向的一维高斯函数对图像滤波,第二次再使用水平方向的一维高斯函数滤波


 */