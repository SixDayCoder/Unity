using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadFromFile : MonoBehaviour {


    private string sharePath = "AssetBundles/share.unity3d";
    private string cubePath = "AssetBundles/prefab/cubewall.unity3d";
    // Use this for initialization
    void Start () {
        //AssetLoadFromFile(); 
        //StartCoroutine(AssetLoadFromMemoryAsync()); 
        //AssetLoadFromMemory();
        //StartCoroutine(LoadFromCacheOrDownload());
        StartCoroutine(WebRequest());
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
	
}
