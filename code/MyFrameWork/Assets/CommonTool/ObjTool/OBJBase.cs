using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OBJModule
{
    public abstract class OBJBase : MonoBehaviour
    {

        public Transform Goal;
        public bool isAutoOpen = false;
        protected bool isOpen = false;
        protected Transform bufGoal;
        public float OpenDelayTime = -1;

        void Awake()
        {
            Init();
        }

        void Update()
        {
            if (this.isOpen) UpdateOBJ();
        }

        /// <summary>
        /// 初始化 
        /// </summary>
        public virtual void Init()
        {
            if (Goal == null) bufGoal = transform;
            else bufGoal = Goal;
        }
        private System.Action act;
        /// <summary>
        /// 开启
        /// </summary>
        public virtual void Open(float delay = 0.0f, System.Action act = null)
        {
            if (act != null) this.act = act;
            if (delay > 0)
            {
                Invoke("OpenDelay", delay);
            }
            else
            {
                if (act != null) act();
            }
        }

        private void OpenDelay()
        {
            if (this.act != null) this.act();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public abstract void UpdateOBJ();

        /// <summary>
        /// 关闭
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 重置
        /// </summary>
        public abstract void ResetOBJ();

        public void SetGoal(Transform goal)
        {
            bufGoal = goal;
        }
    }
}
