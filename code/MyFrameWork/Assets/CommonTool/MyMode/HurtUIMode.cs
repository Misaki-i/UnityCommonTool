/****************************************************
    文件: HurtUIMode.cs
	作者: Misakiか
    邮箱: 1016181268@qq.com
    日期: 2019/8/11 20:40:38
	功能：TODO
*****************************************************/

using System;
using UnityEngine;

namespace CommonTool.MyMode
{
    public class HurtUIMode : MonoBehaviour
    {
        public Transform[] BrokenRoot;
        public Transform[] ScreenEffect;

        private ObjectPool ObjectPool;

        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            ObjectPool = new ObjectPool(GetConfigs(BrokenRoot));
            for (int i = 0; i < BrokenRoot.Length; i++)
            {
                BrokenRoot[i].gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="_transform"></param>
        /// <returns></returns>

        public ObjectPool.ObjectPoolConfig[] GetConfigs(params Transform[] ts)
        {
            ObjectPool.ObjectPoolConfig[] configs =
                new ObjectPool.ObjectPoolConfig[ts.Length];
            for (int i = 0; i < ts.Length; i++)
            {
                configs[i] = ObjectPool.Config("hurt" + i, 20, ts[i].gameObject);
            }
            return configs;
        }

        /// <summary>
        /// 对象池打开伤害特效
        /// </summary>
        /// <param name="localPos"></param>
        /// <param name="name"></param>
        /// <param name="E_max"></param>
        /// <param name="E_min"></param>
        /// <returns></returns>
        public Transform Open(Vector3 localPos, string name, float E_max = 360, float E_min = 0)
        {
            GameObject currentGo = ObjectPool.Open(name);
            currentGo.transform.SetParent(transform);
            currentGo.transform.localScale = Vector3.one;
            currentGo.transform.localPosition = localPos;
            currentGo.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(E_min, E_max));

            currentGo.GetComponent<OBJColor>().Open();
            currentGo.GetComponent<OBJColor>().OnOver = () =>
            {
                ObjectPool.Close(name, currentGo);
            };
            return currentGo.transform;

        }
        /// <summary>
        /// 开启屏幕特效
        /// </summary>
        /// <param name="i"></param>
        public void OpenScreenEffect(int i)
        {
            if (i < ScreenEffect.Length)
            {
                ScreenEffect[i].gameObject.SetActive(true);
                ScreenEffect[i].GetComponent<OBJColor>().Open();
                ScreenEffect[i].GetComponent<OBJColor>().OnOver = () =>
                {
                    ScreenEffect[i].gameObject.SetActive(false);
                };
            }
        }
    }
}
