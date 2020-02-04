using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OBJ位移 
/// </summary>
public class OBJMove : OBJModule.OBJBase
{
    public Ways Way = Ways.Point;
    public enum Ways
    {
        Point,
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
    public Types Type = Types.localPosition;
    public enum Types
    {
        position,
        localPosition,
        offsetPosition,
        offsetLocalPosition
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
    private Vector3 originalPosition;
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
            runTime = 0.0f;

            switch (Type)
            {
                case Types.position:
                    originalPosition = bufGoal.position;
                    break;
                case Types.localPosition:
                    originalPosition = bufGoal.localPosition;
                    break;
                case Types.offsetPosition:
                    originalPosition = bufGoal.position;
                    break;
                case Types.offsetLocalPosition:
                    originalPosition = bufGoal.localPosition;
                    break;
            }
            distance = new Vector3(Target.x - originalPosition.x,
                                   Target.y - originalPosition.y,
                                   Target.z - originalPosition.z);
        });
    }
    public void Open(Vector3 tarPos, float delay = 0.0f, Action overEvent = null)
    {
        base.Open(delay, () =>
        {
            isOpen = true;
            runTime = 0.0f;
            Target = tarPos;
            OnOver = overEvent;

            switch (Type)
            {
                case Types.position:
                    originalPosition = bufGoal.position;
                    break;
                case Types.localPosition:
                    originalPosition = bufGoal.localPosition;
                    break;
                case Types.offsetPosition:
                    originalPosition = bufGoal.position;
                    break;
                case Types.offsetLocalPosition:
                    originalPosition = bufGoal.localPosition;
                    break;
            }
            distance = new Vector3(Target.x - originalPosition.x,
                                   Target.y - originalPosition.y,
                                   Target.z - originalPosition.z);
        });
    }

    public override void UpdateOBJ()
    {
        runTime += Time.deltaTime * Speed;
        switch (Mode)
        {
            case Modes.Normal:
                MoveNormal();
                break;

            case Modes.PingPong:
                MovePingPong();
                break;
        }
    }

    #region 方法
    #region MoveNormal
    public void MoveNormal()
    {
        float bufRunTime = Reverse ? (maxTime - runTime) : runTime;
        float value = 0, xValue = 0, yValue = 0, zValue = 0;
        switch (Way)//决定移动方式
        {
            case Ways.Point:
                value = Curve.Evaluate(bufRunTime);
                break;
            case Ways.Curve:
                xValue = XCurve.Evaluate(bufRunTime);
                yValue = YCurve.Evaluate(bufRunTime);
                zValue = ZCurve.Evaluate(bufRunTime);
                break;
        }

        #region 根据坐标类型和移动方式移动
        switch (Type)
        {
            case Types.position:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.position = originalPosition + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.position = originalPosition +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;

            case Types.localPosition:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.localPosition = originalPosition + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localPosition = originalPosition +
                            new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;

            case Types.offsetPosition:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.position = originalPosition + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.position = originalPosition +
                            new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;

            case Types.offsetLocalPosition:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.localPosition = originalPosition + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localPosition = originalPosition +
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
    #endregion

    #region MovePingPong
    public void MovePingPong()
    {
        float value = 0, xValue = 0, yValue = 0, zValue = 0;
        float bufRunTime;

        if (runTime <= maxTime * 2 * CenterPoint)
        {
            bufRunTime = Reverse ? (maxTime - runTime) : runTime;
            if (!isCenter)
            {
                isCenter = true;
            }
        }
        else
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
            case Ways.Point:
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
            case Types.position:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.position = originalPosition + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.position = originalPosition +
                        new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;

            case Types.localPosition:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.localPosition = originalPosition + distance * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localPosition = originalPosition +
                        new Vector3(distance.x * xValue, distance.y * yValue, distance.z * zValue);
                        break;
                }
                break;

            case Types.offsetPosition:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.position = originalPosition + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.position = originalPosition +
                        new Vector3(Target.x * xValue, Target.y * yValue, Target.z * zValue);
                        break;
                }
                break;

            case Types.offsetLocalPosition:
                switch (Way)
                {
                    case Ways.Point:
                        bufGoal.localPosition = originalPosition + Target * value;
                        break;
                    case Ways.Curve:
                        bufGoal.localPosition = originalPosition +
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

    #endregion

    /// <summary>
    /// 暂停(不重置
    /// </summary>
    public override void Close()
    {
        base.isOpen = false;
    }

    /// <summary>
    /// 重置(不暂停
    /// </summary>
    public override void ResetOBJ()
    {
        switch (Type)
        {
            case Types.position:
            case Types.offsetPosition:
                bufGoal.position = originalPosition;
                break;
            case Types.localPosition:
            case Types.offsetLocalPosition:
                bufGoal.localPosition = originalPosition;
                break;
        }
    }

    /// <summary>
    /// 设置事件时间点
    /// </summary>
    /// <param name="time">百分比时间</param>
    /// <param name="e">事件</param>
    public void SetCenterEvent(float time, Action e)
    {
        CenterPoint = time;
        OnCenter = e;
    }
    #endregion

}
