using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Clock{

    private System.Timers.Timer time;

    private bool isReady = false;//是否完成了一次计时
    private float maxTime = 1.0f;

    public Clock(float maxtime = 1.0f) {
        //毫秒为单位
        ResetClock(maxtime);
    }


    private void Timeout(object source, System.Timers.ElapsedEventArgs e) {
        isReady = true;
    }

    private void ResetClock(float maxtime) {
        isReady = false;
        maxTime = maxtime;
        time = new System.Timers.Timer(maxtime * 1000);
        time.Elapsed += new System.Timers.ElapsedEventHandler(Timeout);
        time.AutoReset = false;//仅计时一次，每次都需要手动开启
        time.Enabled = true;
    }


    public void SetMaxtime(float maxtime) {
        ResetClock(maxtime);
    }

    public bool IsTickComplete() {
        if (isReady) {
            ResetClock(maxTime);//如果没有改变maxtime,仍旧以之前的maxtime来计时
            return true;
        }
        return false;
    }
}
