/****************************************************
    文件: FileTool.cs
	作者: Misakiか
    邮箱: 1016181268@qq.com
    日期: 2019/8/11 20:17:31
	功能：游戏数据保存工具
*****************************************************/


namespace CommonTool
{
    using System.IO;
    using UnityEngine;


    public abstract class FileTool<T> : SingletonMode<T> where T : FileTool<T>
    {
        /// <summary>
        /// 文件保存地址
        /// </summary>
        private string rootPath
        {
            get
            {
                string res = "";
                if (Application.platform == RuntimePlatform.Android) res = Application.persistentDataPath + "/" + Version + "/";
                else if (Application.platform == RuntimePlatform.WindowsEditor) res = Application.streamingAssetsPath + "/" + Version + "/";
                return res;
            }
        }

        #region 版本号
        /// <summary>
        /// 当前版本号
        /// </summary>
        public string Version { get { return version; } }
        private string version = "0.0.1";

        #endregion

        /// <summary>
        /// 文件保存
        /// </summary>
        public struct GlobalJson
        {
            public string[] Values;
        }

        protected void SaveFile(string fileName, GlobalJson json)
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            if (!System.IO.File.Exists(rootPath + fileName))
            {
                FileStream fs = System.IO.File.Create(rootPath + fileName);
                fs.Close();     //关闭
                fs.Dispose();   //释放
            }

            StreamWriter sw = new StreamWriter(rootPath + fileName);
            string jsonString = JsonUtility.ToJson(json);
            sw.Write(jsonString);
            sw.Flush();     //刷新数据缓存
            sw.Close();
            sw.Dispose();
        }

        protected GlobalJson LoadFile(string fileName, int jsonLength, System.Action FileError = null)
        {
            GlobalJson res;
            res.Values = new string[jsonLength];

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            if (!System.IO.File.Exists(rootPath + fileName))
            {
                FileStream fs = System.IO.File.Create(rootPath + fileName);
                fs.Close();     //关闭
                fs.Dispose();   //释放
            }

            StreamReader sr = new StreamReader(rootPath + fileName);
            string context = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            try
            {
                res = JsonUtility.FromJson<GlobalJson>(context);
                if (res.Values.Length != jsonLength)
                {
                    if (FileError != null) FileError();
                }
            }
            catch
            {
                if (res.Values.Length != jsonLength)
                {
                    if (FileError != null) FileError();
                }
            }
            return res;

        }

        //实现抽象类
        public abstract void Save(GlobalJson json);
        public abstract GlobalJson Load();
        public abstract GlobalJson Default();

    }
}
