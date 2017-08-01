using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HideButton : MonoBehaviour {

    public RectTransform imageTransform;
    private Tweener tweener;
    private bool isComing = true;

    private void Start() {
        tweener = imageTransform.DOLocalMove(new Vector3(0, 0, 0), 2f);
        tweener.SetEase(Ease.InBack);//设置动画曲线
        //tweener.SetLoops(2);//设置循环次数 
        tweener.OnComplete(OnComplete);//动画播放完成触发的事件
        tweener.SetAutoKill(false);
        tweener.Pause();
    }


    //每次调用DoXX方法,DOTween都会创建一个Tweener类型的对象,保存动画,默认是该对象是会被自动销毁的
    //合理的方法是在start里设置一个Tweener对象
    public void OnClick() {
        if (isComing) {
            tweener.PlayForward();    
            isComing = false;
        }
        else {
            tweener.PlayBackwards();
            isComing = true;
        }
        
    }

    public void OnComplete() {
        Debug.Log("Completed");
    }
}
