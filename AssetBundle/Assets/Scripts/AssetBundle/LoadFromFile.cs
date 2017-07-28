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
    

    void Start () {
        //AssetLoadFromFile(); 
        //StartCoroutine(AssetLoadFromMemoryAsync()); 
        //AssetLoadFromMemory();
        //StartCoroutine(LoadFromCacheOrDownload());
        //StartCoroutine(WebRequest());
        LoadWithManifest();
	}


    void AssetLoadFromFile() {//也有异步的方法
        //相对路径
        AssetBundle share = AssetBundle.LoadFromFile(sharePath);
        //加载是被加载到内存当中
        AssetBundle ab = AssetBundle.LoadFromFile(cubePath);
        //加载assetbundle,如果没有加载它的依赖是不能正确显示cubeWall的, 不存在加载的先后顺序,在LoadAsset之前即可
        GameObject cubeWall = ab.LoadAsset<GameObject>("CubeWall");//获取prefab
        Instantiate(cubeWall, Vector3.zero, Quaternion.identity);//实例化prefab
    }

    void AssetLoadFromMemory() {
        AssetBundle share = AssetBundle.LoadFromMemory(File.ReadAllBytes(sharePath));
        AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(cubePath));
        GameObject cubeWall = ab.LoadAsset<GameObject>("CubeWall");//获取prefab
        Instantiate(cubeWall, Vector3.zero, Quaternion.identity);//实例化prefab
    }

    IEnumerator AssetLoadFromMemoryAsync() {
        //从内存中异步加载,如果服务器使用的tcp协议传输的是byte数组到本地,那么可以使用这个方法读取AssetBundle到内存
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(sharePath));
        yield return request;//等待加载完share


        request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(cubePath));
        yield return request;//等待加载完request

        //加载完毕,可以使用
        AssetBundle ab = request.assetBundle;
        GameObject cubeWall = ab.LoadAsset<GameObject>("CubeWall");//获取prefab
        Instantiate(cubeWall, Vector3.zero, Quaternion.identity);//实例化prefab
    }

    //WWW,被UnityWenRequest所代替
    IEnumerator LoadFromCacheOrDownload() {
        while (!Caching.ready)
            yield return null;

        //若第一次下载,下载到cache中,之后从cache中取,注意使用www的时候要完整路径
        WWW www = WWW.LoadFromCacheOrDownload(@"file://E:\UnityDemos\AssetBundle\AssetBundles\share.unity3d", 1);
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            www = WWW.LoadFromCacheOrDownload(@"file://E:\UnityDemos\AssetBundle\AssetBundles\prefab\cubewall.unity3d", 1);
            yield return www;

            //加载完毕,可以使用,如果是远程加载,换成服务器的url就可以
            AssetBundle ab = www.assetBundle;
            GameObject cubeWall = ab.LoadAsset<GameObject>("CubeWall");//获取prefab
            Instantiate(cubeWall, Vector3.zero, Quaternion.identity);//实例化prefab

        }
        else {
            Debug.Log(www.error);
            yield break;//相当于void的return
        }

    }

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
        string[] assetBundleNames = manifest.GetAllAssetBundles();

        //想加载某个AssetBundle,加载它所有的依赖AssetBundle,这些依赖关系存在于manifest里边
        
        //加载了被共享的AssetBundle
        foreach(var name in assetBundleNames) {
            if (name =="share.unity3d") {
                AssetBundle.LoadFromFile("AssetBundles/" + name);
                break;
            }
        }

        AssetBundle ab = AssetBundle.LoadFromFile(cubePath);
        Instantiate(ab.LoadAsset<GameObject>("CubeWall"), Vector3.zero, Quaternion.identity);
    }
}
