using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 未完成
/// </summary>
public class OBJAngle : OBJModule.OBJBase
{
    public Ways Way = Ways.Value;
    public enum Ways
    {
        Value,
        Curve,
    }
    public Vector3 Target;
    public AnimationCurve Curve;
    public float Speed = 1f;
    public bool Loop = false;
    public int LoopCount = -1;
    public bool Reverse = false;
    public AnimationCurve XCurve;
    public AnimationCurve YCurve;
    public AnimationCurve ZCurve;
    public Types Type = Types.offsetLocalRotate;
    public enum Types
    {
        rotate,
        localRotate,
        offsetRotate,
        offsetLocalRotate
    }
    public Modes Mode = Modes.Normal;
    public enum Modes
    {
        Normal,
        PingPong
    }

    public Action OnOver;
    public Action OnLoop;
    public Action OnCenter;

    private float runTime = 0.0f;
    private float maxTime = 1.0f;
    private int bufLoopCount = 0;
    private Vector3 distance;
    private Vector3 originalAngle;
    private bool isCenter;
    private float CenterPoint;

    public override void Init()
    {
        base.Init();
        bufLoopCount = LoopCount;
        if (isAutoOpen) { Open(OpenDelayTime); }
        if (OnLoop != null & OnOver == null) { OnOver = OnLoop; }
        CenterPoint = 0.5f;
    }

    public void Open(float delay = 0.0f)
    {
        base.Open(delay, () =>
        {
            isOpen = true;
            runTime = 0f;

            switch (Type)
            {
                case Types.rotate:
                    originalAngle = bufGoal.eulerAngles;
                    break;
                case Types.localRotate:
                    originalAngle = bufGoal.localEulerAngles;
                    break;
                case Types.offsetRotate:
                    originalAngle = bufGoal.eulerAngles;
                    break;
                case Types.offsetLocalRotate:
                    originalAngle = bufGoal.localEulerAngles;
                    break;
            }
            distance = new Vector3(Target.x - originalAngle.x,
                                   Target.y - originalAngle.y,
                                   Target.z - originalAngle.z);
        });
    }

    public override void UpdateOBJ()
    {
        runTime += Time.deltaTime * Speed;
        switch (Mode)
        {
            case Modes.Normal:
                RotateNormal();
                break;

            case Modes.PingPong:
                RotatePingpong();
                break;
        }
    }

    public void RotateNormal()
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
            case Types.rotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.eulerAngles = originalAngle + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.eulerAngles = originalAngle +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.localRotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localEulerAngles = originalAngle + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localEulerAngles = originalAngle +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.offsetRotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.eulerAngles = originalAngle + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.eulerAngles = originalAngle +
                            new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;
            case Types.offsetLocalRotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localEulerAngles = originalAngle + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localEulerAngles = originalAngle +
                            new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;
            default:
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
    }

    public void RotatePingpong()
    {
        float value = 0, xValue = 0, yValue = 0, zValue = 0;
        float bufRunTime;

        if (runTime <= maxTime * 2 * CenterPoint)
        {   //顺
            bufRunTime = Reverse ? (maxTime - runTime) : runTime;
            if (!isCenter)
            {
                isCenter = true;
            }
        }
        else
        {   //逆
            bufRunTime = Reverse ? (runTime - maxTime) : (maxTime * 2 - runTime);
            if (isCenter)
            {
                isCenter = false;
                if (OnCenter != null) { OnCenter(); }
            }
        }

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
            case Types.rotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.eulerAngles = originalAngle + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.eulerAngles = originalAngle +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.localRotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localEulerAngles = originalAngle + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localEulerAngles = originalAngle +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;
            case Types.offsetRotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.eulerAngles = originalAngle + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.eulerAngles = originalAngle +
                            new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;
            case Types.offsetLocalRotate:
                switch (Way)
                {
                    case Ways.Value:
                        bufGoal.localEulerAngles = originalAngle + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localEulerAngles = originalAngle +
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

    public override void Close()
    {
        isOpen = false;
    }

    public override void ResetOBJ()
    {
        transform.localEulerAngles = originalAngle;
    }
    
    public void SetCenterEvent(float time, Action e)
    {
        CenterPoint = time;
        OnCenter = e;
    }
}
