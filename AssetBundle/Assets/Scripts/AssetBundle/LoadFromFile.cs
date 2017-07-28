using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFromFile : MonoBehaviour {
    /*
     * 推荐的方式
     * 1.经常需要更新的资源打包
     * 2.在经常需要更新的资源中,被共同依赖的单独打包到share当中  share/material  share/audioclip share/lua 的目录结构
     * 3.使用UnityWebRequest的方式从远端获取,使用AssetBundleManifest增加可控性
     * 4.如果share的文件很多,而我只想获得x的依赖,那么使用GetAllDependence的方式先加载
     */
    private string sharePath = "AssetBundles/share.unity3d";
    private string cubePath = "AssetBundles/prefab/cubewall.unity3d";

    private AssetBundleManifest MainManifest = null;


    void Start () {
        StartCoroutine(GetMainManifest());//获取MainManifest
        StartCoroutine(LoadAsset("prefab/cubewall.unity3d", "CubeWall"));
	}


    IEnumerator GetMainManifest() {

        string url = @"http://localhost/AssetBundles/AssetBundles";

        UnityWebRequest request = UnityWebRequest.GetAssetBundle(url);
        yield return request.Send();
        if (string.IsNullOrEmpty(request.error)) {
            AssetBundle assetBundle = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            MainManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        else {
            //LogError()
            yield break;
        }
    }


    IEnumerator LoadDependencies(string assetName) {

        string[] dependencies = MainManifest.GetAllDependencies(assetName);

        foreach (var name in dependencies) {
            string url = "http://localhost/AssetBundles/" + name;
            UnityWebRequest request = UnityWebRequest.GetAssetBundle(url);
            yield return request.Send();
            if (string.IsNullOrEmpty(request.error)) {
                AssetBundle assetBundle = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            }
            else {
                //LogError()
                yield break;
            }
        }
    }

    IEnumerator LoadAsset(string assetName, string objName) {

        while (MainManifest == null)
            yield return null;
        StartCoroutine(LoadDependencies(assetName));

        string url = "http://localhost/AssetBundles/" + assetName;
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(url);
        yield return request.Send();//发送http请求

        if (string.IsNullOrEmpty(request.error)) {
            AssetBundle assetBundle = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            GameObject go = ab.LoadAsset<GameObject>(objName);
            Instantiate(go, Vector3.zero, Quaternion.identity);//实例化prefab
        }
        else {
            //LogError()
            yield break;
        }
    }

    /*
    //Unity5.3及以上推荐的方式
    IEnumerator WebRequest() {
        string shareURL = @"http://localhost/AssetBundles/share.unity3d";
        string cubeURL = @"http://localhost/AssetBundles/prefab/cubewall.unity3d";

        UnityWebRequest request = UnityWebRequest.GetAssetBundle(shareURL);
        yield return request.Send();//发送http请求
        AssetBundle share = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        request = UnityWebRequest.GetAssetBundle(cubeURL);
        yield return request.Send();//发送http请求
        
        AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        GameObject cubeWall = ab.LoadAsset<GameObject>("CubeWall");//获取prefab
        Instantiate(cubeWall, Vector3.zero, Quaternion.identity);//实例化prefab
    }


    void LoadWithManifest() {
        
        //主manifest文件,从这个文件中可以获取本项目所有的AssetBundle的信息,这些信息存储在manifest里边
        AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile("AssetBundles/AssetBundles");
        AssetBundleManifest manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //得到了本项目中所有AssetBundle的名字
        //string[] assetBundleNames = manifest.GetAllAssetBundles();
        //想加载某个AssetBundle,加载它所有的依赖AssetBundle,这些依赖关系存在于主manifest里边

        //加载所有依赖
        string[] dependencies = manifest.GetAllDependencies("prefab/cubewall.unity3d");

        foreach(var name in dependencies) {
            AssetBundle.LoadFromFile("AssetBundles/" + name);
        }
        //加载所有依赖
        AssetBundle ab = AssetBundle.LoadFromFile(cubePath);
        Instantiate(ab.LoadAsset<GameObject>("CubeWall"), Vector3.zero, Quaternion.identity);
    }
    */
}
