using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OBJColorUI : OBJColor
{
    public Types Type = Types.Image;
    public Transform[] Group;

    public enum Types //根据shader的变量名,可添加
    {
        Image,
        Text,
        Sprite,
        Shadow,
        RawImage,
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        if (Group.Length == 0)
        {
            if (Goal == null) { bufGoal = transform; }
            else { bufGoal = Goal; }
            Group = new Transform[1] { bufGoal };
        }
        originalColor = new Color[Group.Length];
        distanceColor = new Color[Group.Length];
        for (int i = 0; i < Group.Length; i++)
        {
            switch (Type)
            {
                case Types.Image:
                    originalColor[i] = Group[i].GetComponent<Image>().color; //保存初始颜色
                    break;
                case Types.Text:
                    originalColor[i] = Group[i].GetComponent<Text>().color;
                    break;
                case Types.Sprite:
                    originalColor[i] = Group[i].GetComponent<SpriteRenderer>().color;
                    break;
                case Types.Shadow:
                    originalColor[i] = Group[i].GetComponent<Shadow>().effectColor;
                    break;
                case Types.RawImage:
                    originalColor[i] = Group[i].GetComponent<RawImage>().color;
                    break;
            }
        }
        base.Init();
    }

    public override void ColorNormal()
    {
        float bufRunTime = Reverse ? (maxTime - runTime) : runTime;
        float value = 0, r = 0, g = 0, b = 0, a = 0;
        Color[] bufColors = new Color[Group.Length];

        switch (Way)
        {
            case Ways.Value:
                value = Curve.Evaluate(bufRunTime);
                for (int i = 0; i < Group.Length; i++)
                {
                    bufColors[i] = originalColor[i] + distanceColor[i] * value;
                }
                break;
            case Ways.Curve:
                r = RCurve.Evaluate(bufRunTime);
                g = GCurve.Evaluate(bufRunTime);
                b = BCurve.Evaluate(bufRunTime);
                a = ACurve.Evaluate(bufRunTime);
                for (int i = 0; i < Group.Length; i++)
                {
                    Color color = new Color(r, g, b, a);
                    bufColors[i] = originalColor[i] + distanceColor[i] * color;
                }
                break;
        }

        switch (Type)
        {
            case Types.Image:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<Image>().color = bufColors[i];
                }
                break;
            case Types.Text:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<Text>().color = bufColors[i];
                }
                break;
            case Types.Sprite:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<SpriteRenderer>().color = bufColors[i];
                }
                break;
            case Types.Shadow:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<Shadow>().effectColor = bufColors[i];
                }
                break;
            case Types.RawImage:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<RawImage>().color = bufColors[i];
                }
                break;
        }

        #region 半程判断
        if (runTime <= maxTime * 0.5f)
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

        #region 循环判断
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

    public override void ColorPingpong()
    {
        float bufRunTime;
        float value = 0, r = 0, g = 0, b = 0, a = 0;
        Color[] bufColors = new Color[Group.Length];

        if (runTime <= maxTime)
        {
            //前半段
            bufRunTime = Reverse ? (maxTime - runTime) : runTime;
            if (!isCenter)
            {
                isCenter = true;
            }
        }
        else
        {
            bufRunTime = Reverse ? (runTime - maxTime) : (maxTime * 2 - runTime);
            //后半段
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
                for (int i = 0; i < Group.Length; i++)
                {
                    bufColors[i] = originalColor[i] + distanceColor[i] * value;
                }
                break;
            case Ways.Curve:
                r = RCurve.Evaluate(bufRunTime);
                g = GCurve.Evaluate(bufRunTime);
                b = BCurve.Evaluate(bufRunTime);
                a = ACurve.Evaluate(bufRunTime);
                for (int i = 0; i < Group.Length; i++)
                {
                    Color color = new Color(r, g, b, a);
                    bufColors[i] = originalColor[i] + distanceColor[i] * color;
                }
                break;
        }

        switch (Type)
        {
            case Types.Image:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<Image>().color = bufColors[i];
                }
                break;
            case Types.Text:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<Text>().color = bufColors[i];
                }
                break;
            case Types.Sprite:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<SpriteRenderer>().color = bufColors[i];
                }
                break;
            case Types.Shadow:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<Shadow>().effectColor = bufColors[i];
                }
                break;
            case Types.RawImage:
                for (int i = 0; i < Group.Length; i++)
                {
                    Group[i].GetComponent<RawImage>().color = bufColors[i];
                }
                break;
        }

        #region 循环判断
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

    public override void ResetOBJ()
    {
        for (int i = 0; i < Group.Length; i++)
        {
            switch (Type)
            {
                case Types.Image:
                    Group[i].GetComponent<Image>().color = originalColor[i];
                    break;
                case Types.Text:
                    Group[i].GetComponent<Text>().color = originalColor[i];
                    break;
                case Types.Sprite:
                    Group[i].GetComponent<SpriteRenderer>().color = originalColor[i];
                    break;
                case Types.Shadow:
                    Group[i].GetComponent<Shadow>().effectColor = originalColor[i];
                    break;
                case Types.RawImage:
                    Group[i].GetComponent<RawImage>().color = originalColor[i];
                    break;
            }
        }
    }
}
