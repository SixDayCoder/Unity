using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class PostEffectBase : MonoBehaviour {

    // Use this for initialization
    void Start() {
        CheckResources();
    }

    protected void CheckResources() {
        bool isSupported = CheckSupport();
        if (!isSupported) {
            enabled = false;
        }
    }

    protected bool CheckSupport() {
        if (!SystemInfo.supportsImageEffects) {
            Debug.LogWarning("This platform does not support image effects");
            return false;
        }
        return true;
    }

    protected Material CheckShaderAndCreateMaterial(Shader shader, Material mat) {
        if (shader == null)
            return null;
        if (!shader.isSupported)
            return null;
        if (shader.isSupported && mat && mat.shader == shader)
            return mat;

        mat = new Material(shader);
        mat.hideFlags = HideFlags.DontSave;//自己管理,不会保存到场景,即便新场景加载也不会被销毁
        if (mat) return mat;
        else
            return null;
    }

}
