
namespace CommonTool.GameStatus
{
    /*
        状态机模块功能
        --》状态机基本功能：状态进入，切换，传参
        --》计时器功能：使用下标操作计时器对象，实现开启一次和循环开启功能

        PS使用
        --》状态机：1、新建StatusManger控制状态切换
                    2、新建状态对象存入StatusManager中
                    3、Update调用StatusManager的Update函数

         */

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// 状态控制器
    /// </summary>
    public class StatusManager
    {

        public StatusManager(){ }

        private string currentStateName;        //当前状态名字
        private StatusBase currentState;        //当前状态对象

        /// <summary>
        /// 键值存储状态对象
        /// </summary>
        private Dictionary<string, StatusBase> StateDic = new Dictionary<string, StatusBase>();
        /// <summary>
        /// 键值存储计时器(用于处理逻辑中需要控制的计时器对象)
        /// </summary>
        private Dictionary<string, TimerTool> TimerDic = new Dictionary<string, TimerTool>();
        /// <summary>
        /// 集合存储计时器
        /// </summary>
        private TimerTool[] TimerList;




        #region 状态机
        /// <summary>
        /// 初始化状态
        /// </summary>
        /// <param name="status"></param>
        public void InitStatus(StatusBase status, object param = null)
        {
            currentStateName = status.StatusName;
            currentState = status;
            currentState.OnEntry(param);
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="status">状态对象</param>
        public void AddStatus(StatusBase status)
        {
            if (status == null)
            {
                Debug.Log("状态为空....");
                return;
            }
            if (StateDic.ContainsValue(status))
            {
                Debug.Log("存在此状态。。。");
                return;
            }
            StateDic.Add(status.StatusName, status);
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="status">状态对象</param>
        public void DeleteStatus(StatusBase status)
        {
            if (status == null)
            {
                Debug.Log("状态为空。。。。");
                return;
            }
            if (!StateDic.ContainsValue(status))
            {
                Debug.Log("不存在此状态。。。。");
                return;
            }
            StateDic.Remove(status.StatusName);
        }

        /// <summary>
        /// 按状态名字进行切换
        /// </summary>
        /// <param name="_statusName">状态名</param>
        public void ChangeStatus(string _statusName, object param = null)
        {
            currentStateName = _statusName;
            currentState.OnLeave(param);
            StateDic.TryGetValue(_statusName, out currentState);
            currentState.OnEntry(param);
        }

        /// <summary>
        /// 更新函数，需要在Update里调用
        /// </summary>
        public void UpdateSystem()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }
        #endregion

        #region Timer模式1（Dictionary）
        ///// <summary>
        ///// 状态机中添加计时对象
        ///// </summary>
        ///// <param name="timerName">键</param>
        //public void AddTimer(string timerName)
        //{
        //    if (!TimerDic.ContainsKey(timerName)) 
        //    {
        //        TimerSingle timer = new TimerSingle();
        //        TimerDic.Add(timerName, timer);
        //    }
        //    else
        //    {
        //        Debug.Log("TimerDic字典中已经存在");
        //        return;
        //    }
        //}
        ///// <summary>
        ///// 计时器开启一次
        ///// </summary>
        ///// <param name="timerName">键</param>
        ///// <param name="time">时间</param>
        ///// <param name="func">回调</param>
        //public void TimeOpenOnce(string timerName, float time, System.Action func)
        //{
        //    if (TimerDic.ContainsKey(timerName))
        //    {
        //        TimerSingle timer;
        //        TimerDic.TryGetValue(timerName, out timer);
        //        timer.StatusOpenOnce(time, func);
        //    }
        //    else
        //    {
        //        Debug.Log("不存在的Timer");
        //        return;
        //    }
        //}
        ///// <summary>
        ///// 计时器循环开启
        ///// </summary>
        ///// <param name="timerName">键</param>
        ///// <param name="time">多少时间循环一次</param>
        ///// <param name="loopCount">循环次数</param>
        ///// <param name="func">回调</param>
        //public void TimeOpenLoop(string timerName, float time, int loopCount, System.Action func)
        //{
        //    if (TimerDic.ContainsKey(timerName))
        //    {
        //        TimerSingle timer;
        //        TimerDic.TryGetValue(timerName, out timer);
        //        timer.StatusOpenLoop(time, loopCount, func);
        //    }
        //    else
        //    {
        //        Debug.Log("不存在的Timer");
        //        return;
        //    }
        //}


        #endregion

        #region Timer模式2（数组）
        /// <summary>
        /// 计时器初始化
        /// </summary>
        /// <param name="num">计时器集合大小</param>
        public void InitTimer(int num = 16)
        {
            TimerList = new TimerTool[num];
        }
        /// <summary>
        /// 计时器开启一次
        /// </summary>
        /// <param name="key">下标</param>
        /// <param name="time">时间</param>
        /// <param name="func">回调</param>
        public void TimerOpenOnce(int key, float time, System.Action func)
        {
            if (key < 0) return;

            if (key < TimerList.Length && TimerList[key] == null) 
            {
                TimerTool timer = TimerTool.OpenOnce(time, func);
                TimerList[key] = timer;
            }
            else
            {
                TimerList[key].StatusOpenOnce(time, func);
            }
        }
        /// <summary>
        /// 计时器循环开启
        /// </summary>
        /// <param name="key">下标</param>
        /// <param name="time">时间间隔</param>
        /// <param name="loopCount">循环次数</param>
        /// <param name="func">回调</param>
        public void TimerOpenLoop(int key, float time, int loopCount, System.Action func)
        {
            if (key < 0) return;

            if (key < TimerList.Length && TimerList[key] == null)
            {
                TimerTool timer = TimerTool.OpenLoop(time, loopCount, func);
                TimerList[key] = timer;
            }
            else
            {
                if (TimerList[key].IsOpen) return;
                TimerList[key].StatusOpenLoop(time, loopCount, func);
            }
        }
        /// <summary>
        /// 暂停计时器
        /// </summary>
        /// <param name="key">下标</param>
        public void PauseTimer(int key)
        {
            if (key < 0) return;

            if (key < TimerList.Length && TimerList[key] != null)
            {
                TimerList[key].Pause();
            }
        }
        /// <summary>
        /// 继续计时器
        /// </summary>
        /// <param name="key">下标</param>
        public void ContinueTimer(int key)
        {
            if (key < 0) return;

            if (key < TimerList.Length && TimerList[key] != null)
            {
                TimerList[key].Continue();
            }
        }
        /// <summary>
        /// 关闭计时器
        /// </summary>
        /// <param name="key">下标</param>
        public void CloseTimer(int key)
        {
            if (key < 0) return;

            if (key < TimerList.Length && TimerList[key] != null)
            {
                TimerList[key].Stop();
            }
        }


        #endregion


    }
}

