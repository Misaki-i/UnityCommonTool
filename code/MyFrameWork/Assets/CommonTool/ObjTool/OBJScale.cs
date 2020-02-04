using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OBJ缩放
/// </summary>
public class OBJScale : OBJModule.OBJBase
{
    public Ways Way = Ways.Value;
    public enum Ways
    {
        Value,
        Curve,
    }
    public Vector3 Target;
    public AnimationCurve Curve;
    public float Speed = 1.0f;
    public bool Loop = false;
    public int LoopCount = -1;
    public bool Reverse = false;
    public AnimationCurve XCurve;
    public AnimationCurve YCurve;
    public AnimationCurve ZCurve;
    public Types Type = Types.localScale;
    public enum Types
    {
        localScale,         //值  变化到特定值
        localScaleX,        //倍  变化到特定倍数
        localScaleOffset,   //差  变化特定值
    }
    public Modes Mode = Modes.Normal;
    public enum Modes
    {
        Normal,
        PingPong,
    }

    public Action OnOver;
    public Action OnLoop;
    public Action OnCenter;

    private float runTime = 0.0f;
    private float maxTime = 1.0f;
    private int bufLoopCount = 0;
    private Vector3 distance;
    private Vector3 originalScale;
    private bool isCenter;
    private float CenterPoint;

    public override void Init()
    {
        base.Init();
        bufLoopCount = LoopCount;
        if (isAutoOpen) Open(OpenDelayTime);
        if (OnLoop != null & OnOver == null) { OnOver = OnLoop; }
        CenterPoint = 0.5f;
        originalScale = bufGoal.localScale;
    }

    public void Open(float delay = 0.0f)
    {
        base.Open(delay, () =>
        {
            isOpen = true;
            runTime = 0.0f;

            switch (Type)
            {
                case Types.localScale:
                    distance = new Vector3(Target.x - originalScale.x,
                                           Target.y - originalScale.y,
                                           Target.z - originalScale.z);
                    break;
                case Types.localScaleX:
                case Types.localScaleOffset:
                    distance = new Vector3(Target.x * originalScale.x - originalScale.x,
                                           Target.y * originalScale.y - originalScale.y,
                                           Target.z * originalScale.z - originalScale.z);
                    break;
            }
        });
    }

    public override void UpdateOBJ()
    {
        runTime += Time.deltaTime * Speed;
        switch (Mode)
        {
            case Modes.Normal: //普通
                ScaleNormal();
                break;

            case Modes.PingPong: //普通+回放
                ScalePingPong();
                break;
        }
    }

    #region 方法
    /// <summary>
    /// 按曲线大小变化
    /// </summary>
    public void ScaleNormal()
    {
        float bufRunTime = Reverse ? (maxTime - runTime) : runTime;
        float value = 0, xValue = 0, yValue = 0, zValue = 0;
        switch (Way)
        {
            case Ways.Value:
                value = Curve.Evaluate(bufRunTime);
                break;
            case Ways.Curve:
                xValue = XCurve.Evaluate(bufRunTime);
                yValue = YCurve.Evaluate(bufRunTime);
                zValue = ZCurve.Evaluate(bufRunTime);
                break;
        }

        switch (Type)
        {
            case Types.localScale:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localScale = originalScale + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localScale = originalScale +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.localScaleX:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localScale = originalScale + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localScale = originalScale +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.localScaleOffset:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localScale = originalScale + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localScale = originalScale +
                            new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;
        }

        #region 半程判断
        if (runTime <= maxTime * CenterPoint)
        {
            if (!isCenter)
            {
                isCenter = true;
            }
        }
        else
        {
            if (isCenter)
            {
                isCenter = false;
                if (OnCenter != null) { OnCenter(); }
            }
        }
        #endregion

        #region 循环与结束判断
        if (runTime >= maxTime)
        {
            if (!Loop)
            {
                isOpen = false;
                if (OnOver != null) OnOver();
            }
            else
            {
                bufLoopCount--;
                if (bufLoopCount > 0 | bufLoopCount < 0)
                {
                    runTime = 0f;
                    if (OnLoop != null) OnLoop();
                }
                else
                {
                    isOpen = false;
                    if (OnOver != null) OnOver();
                    bufLoopCount = LoopCount;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 正向变化后方向变化
    /// </summary>
    public void ScalePingPong()
    {
        float value = 0, xValue = 0, yValue = 0, zValue = 0;
        float bufRunTime;

        if (runTime <= maxTime * 2 * CenterPoint)   //↓↓↓↓↓↓↓↓↓↓正向↓↓↓↓↓↓↓↓↓↓↓↓↓
        {
            bufRunTime = Reverse ? (maxTime - runTime) : runTime;
            if (!isCenter)
            {
                isCenter = true;
            }
        }
        else   //↓↓↓↓↓↓↓↓↓↓反向↓↓↓↓↓↓↓↓↓↓↓↓↓
        {
            bufRunTime = Reverse ? (runTime - maxTime) : (maxTime * 2 - runTime);
            if (isCenter)
            {
                isCenter = false;
                if (OnCenter != null) { OnCenter(); }
            }
        }

        switch (Way)//决定移动方式
        {
            case Ways.Value:
                value = Curve.Evaluate(bufRunTime);
                break;
            case Ways.Curve:
                xValue = XCurve.Evaluate(bufRunTime);
                yValue = YCurve.Evaluate(bufRunTime);
                zValue = ZCurve.Evaluate(bufRunTime);
                break;
        }

        switch (Type)
        {
            case Types.localScale:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localScale = originalScale + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localScale = originalScale +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.localScaleX:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localScale = originalScale + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localScale = originalScale +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.localScaleOffset:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localScale = originalScale + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localScale = originalScale +
                            new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;
        }

        #region 循环与结束判断
        if (runTime >= maxTime * 2)
        {
            if (!Loop)
            {
                isOpen = false;
                if (OnOver != null) OnOver();
            }
            else
            {
                bufLoopCount--;
                if (bufLoopCount > 0 | bufLoopCount < 0)
                {
                    runTime = 0f;
                    if (OnLoop != null) OnLoop();
                }
                else
                {
                    isOpen = false;
                    if (OnOver != null) OnOver();
                    bufLoopCount = LoopCount;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 暂停(不重置
    /// </summary>
    public override void Close()
    {
        isOpen = false;
    }

    /// <summary>
    /// 重置(不暂停
    /// </summary>
    public override void ResetOBJ()
    {
        bufGoal.transform.localScale = originalScale;
    }

    public void SetCenterEvent(float time, Action e)
    {
        CenterPoint = time;
        OnCenter = e;
    }
    #endregion
}
