using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJColorMaterial : OBJColor
{
    private Material[] material;

    public ColorType Type = ColorType._Emission;
    public Transform[] Group;

    public enum ColorType //根据shader的变量名,可添加
    {
        _Color,
        _Emission,
        _TintColor,
        _EmissionColor,
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        isCenter = true;
        if (Group.Length == 0)
        {
            if (Goal == null)
            {
                bufGoal = transform;
            }
            else
            {
                bufGoal = Goal;
            }
            Group = new Transform[1] { bufGoal };
        }
        
        List<Material> mList = new List<Material>();
        for (int i = 0; i < Group.Length; i++)
        {
            if (Group[i].GetComponent<MeshRenderer>() != null)
            {
                int l = Group[i].GetComponent<MeshRenderer>().materials.Length;
                for (int j = 0; j < l; j++)
                {
                    mList.Add(Group[i].GetComponent<MeshRenderer>().materials[j]);
                }
            }
            if (Group[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                int l = Group[i].GetComponent<SkinnedMeshRenderer>().materials.Length;
                for (int j = 0; j < l; j++)
                {
                    mList.Add(Group[i].GetComponent<SkinnedMeshRenderer>().materials[j]);
                }
            }
            if (Group[i].GetComponent<TrailRenderer>() != null)
            {
                int l = Group[i].GetComponent<TrailRenderer>().materials.Length;
                for (int j = 0; j < l; j++)
                {
                    mList.Add(Group[i].GetComponent<TrailRenderer>().materials[j]);
                }
            }
            if (Group[i].GetComponent<Renderer>() != null)
            {
                int l = Group[i].GetComponent<Renderer>().materials.Length;
                for (int j = 0; j < l; j++)
                {
                    mList.Add(Group[i].GetComponent<Renderer>().materials[j]);
                }
            }
        }

        material = new Material[mList.Count];
        for (int i = 0; i < mList.Count; i++)
        {
            material[i] = mList[i];
        }

        if (material.Length == 0)
        {
            Debug.LogError("没有材质");
            return;
        }
        originalColor = new Color[material.Length];
        distanceColor = new Color[material.Length];
        for (int i = 0; i < material.Length; i++)
        {
            originalColor[i] = material[i].GetColor(Type.ToString()); //保存初始颜色
        }
        base.Init();
    }

    public override void ColorNormal()
    {
        float bufRunTime = Reverse ? (maxTime - runTime) : runTime; //跑曲线,反向or正向[x轴]
        float value = 0, r = 0, g = 0, b = 0, a = 0;

        switch (Way)
        {
            case Ways.Value:
                value = Curve.Evaluate(bufRunTime);
                for (int i = 0; i < material.Length; i++)
                {
                    material[i].SetColor(Type.ToString(), originalColor[i] + distanceColor[i] * value);
                }
                break;
            case Ways.Curve:
                r = RCurve.Evaluate(bufRunTime);
                g = GCurve.Evaluate(bufRunTime);
                b = BCurve.Evaluate(bufRunTime);
                a = ACurve.Evaluate(bufRunTime);
                for (int i = 0; i < material.Length; i++)
                {
                    Color color = new Color(r, g, b, a);
                    material[i].SetColor(Type.ToString(), originalColor[i] + distanceColor[i] * color);
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

    public override void ColorPingpong()
    {
        float bufRunTime;
        float value = 0, r = 0, g = 0, b = 0, a = 0;

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
                for (int i = 0; i < material.Length; i++)
                {
                    material[i].SetColor(Type.ToString(), originalColor[i] + distanceColor[i] * value);
                }
                break;
            case Ways.Curve:
                r = RCurve.Evaluate(bufRunTime);
                g = GCurve.Evaluate(bufRunTime);
                b = BCurve.Evaluate(bufRunTime);
                a = ACurve.Evaluate(bufRunTime);
                for (int i = 0; i < material.Length; i++)
                {
                    Color color = new Color(r, g, b, a);
                    material[i].SetColor(Type.ToString(), originalColor[i] + distanceColor[i] * color);
                }
                break;
        }

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
    }

    public override void ResetOBJ()
    {
        for (int i = 0; i < material.Length; i++)
        {
            material[i].SetColor(Type.ToString(), originalColor[i]);
        }
    }
}
