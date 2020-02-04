using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数字递增递减参考
/// </summary>
public class LerpTool : MonoBehaviour {

    /// <summary>
    /// 曲线速度设置
    /// </summary>
    /// <param name="to">目标值</param>
    /// <param name="buf">当前操作值</param>
    /// <param name="minSpeed">最小速度</param>
    /// <returns></returns>
    private static float GetSpeed(float to, float buf, float minSpeed)
    {
        float dis = Mathf.Abs(to - buf);
        if (dis < minSpeed)
        {
            dis = minSpeed;
        }
        return dis;
    }
    /// <summary>
    /// 得到递增或递减值
    /// </summary>
    /// <param name="buf">当前操作值</param>
    /// <param name="to">目标</param>
    /// <param name="minSpeed">最小速度</param>
    /// <param name="scaleSpeed">速度倍数</param>
    /// <param name="over">结束事件</param>
    /// <returns></returns>
    public static float GetBufValue(float buf, float to, float minSpeed, float scaleSpeed = 1, System.Action over = null)
    {
        if (buf < to)
        {
            buf += GetSpeed(to, buf, minSpeed) * Time.deltaTime * scaleSpeed;
            if (buf > to)
            {
                buf = to;
                if (over != null)
                {
                    over();
                }
            }
        }
        if (buf > to)
        {
            buf -= GetSpeed(to, buf, minSpeed) * Time.deltaTime * scaleSpeed;
            if (buf < to)
            {
                buf = to;
                if (over != null)
                {
                    over();
                }
            }
        }
        return buf;
    }
}
