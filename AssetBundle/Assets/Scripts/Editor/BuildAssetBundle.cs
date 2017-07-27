using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;


public class BuildAssetBundle{

    [MenuItem("AssetBundle/Build All AssetBundle")]
    static void BuildAllAssetBundle() {
        //BuildPipeline.BuildAssetBundles();
        string path = "AssetBundles";//相对目录,和Asset同级
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //None使用LZMA算法压缩,被压缩的包相比LZ4更多,但是解压时间更久,加载时间更久，解压是必须整体解压
        //ChunkBasedCompressor LZ4压缩 可以指定加载具体的资源而无需全部解压
    }
    /*
     * CubeWall和SphereWall都引用了同一套贴图和材质,直接打包那么会重复
     * 最好是依赖打包，把公用的贴图和材质打包,这样会减少内存 
     * 依赖打包实际上是手动多次分配assetbundle的name多次打包 依赖关系会由unity自行设定
     */


    /*
     * 1.经常更新的资源单独打包，和不经常更新的资源分开
     * 2.被公共引用的资源单独打包
     */

}
