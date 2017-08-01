using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextTween : MonoBehaviour {

    private Text text;
    private Tweener tweener;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.DOText("对镜贴花黄对镜贴花黄对镜贴花黄对镜贴花黄对镜贴花黄对镜贴花黄对镜贴花黄对镜贴花黄", 5f);
        text.DOColor(Color.red, 2.0f);
        text.DOFade(0.0f, 2.0f);//修改color的alpha通道
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
