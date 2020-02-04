
namespace CommonTool.GameStatus
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// 状态基类
    /// </summary>
    public abstract class StatusBase 
    {
        private string statusName;
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get { return statusName; } }

        private StatusManager statusHandle;
        /// <summary>
        /// 状态控制器
        /// </summary>
        public StatusManager StatusHandle { get { return statusHandle; } }

        public StatusBase(StatusManager _statusSystem, string _statusName)
        {
            statusHandle = _statusSystem;
            statusName = _statusName;
        }

        //重写方法

        /// <summary>
        /// 状态进入
        /// </summary>
        public abstract void OnEntry(object param);
        /// <summary>
        /// 状态更新
        /// </summary>
        public abstract void OnUpdate();
        /// <summary>
        /// 状态离开
        /// </summary>
        public abstract void OnLeave(object param);

    }
}

