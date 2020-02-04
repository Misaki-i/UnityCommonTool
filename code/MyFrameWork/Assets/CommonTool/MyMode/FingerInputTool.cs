/****************************************************
    文件: FingerInputTool.cs
	作者: Misakiか
    邮箱: 1016181268@qq.com
    日期: 2019/8/11 20:24:46
	功能：手指输入模块
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace CommonTool.MyMode
{

    /// <summary>
    /// 手指输入模块
    /// </summary>
    public class FingerInputTool : SingletonMode<FingerInputTool>
    {
        private List<Finger> fingerList = new List<Finger>();

        /// <summary>
        /// 手指进入屏幕
        /// </summary>
        public event ActionTool<Finger> OnTouchEntryCallBack = EventTool<Finger>.CreateEvent();

        /// <summary>
        /// 手指离开屏幕
        /// </summary>
        public event ActionTool<Finger> OnTouchLeaveCallBack = EventTool<Finger>.CreateEvent();

        /// <summary>
        /// 手指拖动回调
        /// </summary>
        public event ActionTool<Finger> OnTouchDragCallBack = EventTool<Finger>.CreateEvent();

        /// <summary>
        /// 测试使用
        /// </summary>
        private Vector3 mouseDelta = Vector3.zero;


        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Touch[] touchs = Input.touches;
                fingerList.Clear();
                for (int i = 0; i < touchs.Length; i++)
                {
                    Touch touch = touchs[i];

                    if (touch.fingerId == -1) continue;

                    Finger finger = new Finger(touch.fingerId);

                    fingerList.Add(finger);
                    finger.UpdateDeltaPosition(touch.deltaPosition);
                    finger.UpdatePosition(touch.position);

                    if (touch.phase == TouchPhase.Began)
                    {
                        OnTouchEntryCallBack(finger);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        OnTouchLeaveCallBack(finger);
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        OnTouchDragCallBack(finger);
                    }
                }
            }

            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                fingerList.Clear();
                Finger finger = new Finger(0);//你就一个鼠标遮
                fingerList.Add(finger);
                finger.UpdatePosition(Input.mousePosition);
                Vector3 delta = mouseDelta - Input.mousePosition;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    OnTouchEntryCallBack(finger);
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    OnTouchLeaveCallBack(finger);
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (delta.magnitude > 10)
                    {
                        OnTouchDragCallBack(finger);
                    }
                }

                finger.UpdateDeltaPosition(mouseDelta);
                mouseDelta = Input.mousePosition;
            }

        }

        public class Finger
        {
            /// <summary>
            /// 手指触碰回调
            /// </summary>
            public event ActionTool<Vector3> OnTouchEntryCallBack = EventTool<Vector3>.CreateEvent();

            /// <summary>
            /// 手指拖动回调
            /// </summary>
            public event ActionTool<Vector3> OnTouchDragCallBack = EventTool<Vector3>.CreateEvent();

            /// <summary>
            /// 手指离开回调
            /// </summary>
            public event ActionTool<Vector3> OnTouchLeaveCallBack = EventTool<Vector3>.CreateEvent();

            private int id;

            /// <summary>
            /// ID
            /// </summary>
            public int ID { get { return id; } }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="fid">ID</param>
            public Finger(int fid)
            {
                id = fid;
            }

            private Vector3 position;

            /// <summary>
            /// 位置
            /// </summary>
            public Vector3 Position { get { return position; } }

            /// <summary>
            /// 更新位置
            /// </summary>
            /// <param name="pos">位置</param>
            public void UpdatePosition(Vector3 pos)
            {
                position = pos;
            }

            private Vector3 deltaPosition;

            /// <summary>
            /// 位置差
            /// </summary>
            public Vector3 DeltaPosition { get { return deltaPosition; } }

            /// <summary>
            /// 更新位置差
            /// </summary>
            /// <param name="delta">位置差</param>
            public void UpdateDeltaPosition(Vector3 delta)
            {
                deltaPosition = delta;
            }
        }

    }

}
