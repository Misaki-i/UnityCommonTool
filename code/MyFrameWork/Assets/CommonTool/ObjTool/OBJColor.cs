using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OBJ变色
/// </summary>
public abstract class OBJColor : OBJModule.OBJBase
{
    public Ways Way = Ways.Value;
    public enum Ways
    {
        Value,
        Curve,
    }
    public Color ToColor = Color.white;//目标颜色 RGBA
    public AnimationCurve Curve;//变化曲线 x=>变化比例,y=>1秒时间
    public float Speed = 1f;//速度
    public bool Loop = false;//循环
    public int LoopCount = -1;//循环次数
    public bool Reverse = false;//反向

    public AnimationCurve RCurve;
    public AnimationCurve GCurve;
    public AnimationCurve BCurve;
    public AnimationCurve ACurve;

    public MoveMode Mode = MoveMode.Normal;
    public enum MoveMode
    {
        Normal,
        PingPong
    }
    public Action OnOver;//完成事件
    public Action OnLoop;//循环事件
    public Action OnCenter;//半程事件

    protected float runTime = 0f;
    protected float maxTime = 1f;
    protected bool isCenter;
    protected int bufLoopCount = 0;
    protected MeshRenderer mesh;

    protected Color[] originalColor;//初始颜色
    protected Color[] distanceColor;//到目标颜色的变化量

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        base.Init();
        if (isAutoOpen) { Open(OpenDelayTime); } //自动开启
    }

    /// <summary>
    /// 启动
    /// </summary>
    /// <param name="delay">延时</param>
    public void Open(float delay = 0f)
    {
        bufLoopCount = LoopCount;
        base.Open(delay, () =>
        {
            isOpen = true;
            runTime = 0.0f;
            for (int i = 0; i < originalColor.Length; i++)
            {
                distanceColor[i] = ToColor - originalColor[i]; //计算变化量
            }
        });
    }

    public override void UpdateOBJ()
    {
        runTime += Time.deltaTime * Speed;
        switch (Mode)
        {
            case MoveMode.Normal:
                ColorNormal();
                break;

            case MoveMode.PingPong:
                ColorPingpong();
                break;
        }
    }

    public abstract void ColorNormal();
    public abstract void ColorPingpong();

    public override void Close()
    {
        isOpen = false;
    }
}
