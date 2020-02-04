
namespace CommonTool.iTween
{
    using UnityEngine;
    using System.Collections;
    using CommonTool;
    using System.Collections.Generic;

    /// <summary>
    /// 路径管理工具，使用iTween
    /// </summary>
    public class PathManager : SingletonMode<PathManager>
    {
        public iTweenPath[] Paths;
        private Dictionary<string, iTweenPath> PathsDic;

        protected override void Awake()
        {

            base.Awake();
            //初始化
            for (int i = 0; i < Paths.Length; i++)
            {
                PathsDic.Add(Paths[i].pathName, Paths[i]);
            }
        }

        /// <summary>
        /// 外界获取路径
        /// </summary>
        /// <param name="_pathName">路径名</param>
        /// <returns></returns>
        public Vector3[] GetPath(string _pathName)
        {
            if (PathsDic.ContainsKey(_pathName))
            {
                Vector3[] path = iTweenPath.GetPath(_pathName).ToArray();
                return path;
            }
            else
            {
                Debug.Log("找不到对应的路径:" + _pathName);
                return null;
            }
        }



    }
}

