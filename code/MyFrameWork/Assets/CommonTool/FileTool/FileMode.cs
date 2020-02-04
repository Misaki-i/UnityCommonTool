/****************************************************
    文件: FileMode.cs
	作者: Misakiか
    邮箱: 1016181268@qq.com
    日期: 2019/8/11 20:21:29
	功能：文件保存参考
*****************************************************/
using UnityEngine;

namespace CommonTool
{
    class FileMode : FileTool<FileMode>
    {

        private string fileName = "GlobalFileLevels.json";
        private const int NUMBER_1 = 0;
        private const int NUMBER_2 = 1;
        private const int NUMBER_3 = 2;
        private const int NUMBER_4 = 3;
        private const int NUMBER_5 = 4;
        private const int NUMBER_6 = 5;

        private const int LENGHT = 6;
        /// <summary>
        /// 默认设置
        /// </summary>
        /// <returns>文件数据</returns>
        public override GlobalJson Default()
        {
            GlobalJson json;
            json.Values = new string[LENGHT];
            json.Values[NUMBER_1] = "1";
            json.Values[NUMBER_2] = "2";
            json.Values[NUMBER_3] = "3";
            json.Values[NUMBER_4] = "4";
            json.Values[NUMBER_5] = "5";
            json.Values[NUMBER_6] = "6";
            int n1 = int.Parse(json.Values[NUMBER_1]);
            int n2 = int.Parse(json.Values[NUMBER_2]);
            int n3 = int.Parse(json.Values[NUMBER_3]);
            int n4 = int.Parse(json.Values[NUMBER_4]);
            int n5 = int.Parse(json.Values[NUMBER_5]);
            int n6 = int.Parse(json.Values[NUMBER_6]);

            Save(json);
            int[] lvs = new int[6] { n1, n2, n3, n4, n5, n6 };
            //GameData.SetLvs(lvs);

            return json;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns>文件数据</returns>
        public override GlobalJson Load()
        {
            GlobalJson res;
            res.Values = new string[1];
            bool error = false;
            GlobalJson json = LoadFile(fileName, LENGHT, () =>
            {
                //异常...
                Debug.Log("GlobalFileLevels文件加载失败!!");
                res = Default();
                error = true;
            });
            if (!error) res = json;
            int n1 = int.Parse(res.Values[0]);
            int n2 = int.Parse(res.Values[1]);
            int n3 = int.Parse(res.Values[2]);
            int n4 = int.Parse(res.Values[3]);
            int n5 = int.Parse(res.Values[4]);
            int n6 = int.Parse(res.Values[5]);
            int[] lvs = new int[6] { n1, n2, n3, n4, n5, n6 };
            //GameData.SetLvs(lvs);

            return res;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="json">文件数据</param>
        public override void Save(GlobalJson json)
        {
            GlobalJson saveJson;
            saveJson.Values = new string[json.Values.Length];
            for (int i = 0; i < json.Values.Length; i++)
            {
                saveJson.Values[i] = json.Values[i];
            }
            SaveFile(fileName, saveJson);
        }

        /// <summary>
        /// 保存关卡(外部调用的保存类)
        /// </summary>
        /// <param name="lvs">关卡数组</param>
        public void Save(int[] lvs)
        {
            GlobalJson json;
            json.Values = new string[LENGHT];
            json.Values[NUMBER_1] = lvs[0].ToString("0");
            json.Values[NUMBER_2] = lvs[1].ToString("0");
            json.Values[NUMBER_3] = lvs[2].ToString("0");
            json.Values[NUMBER_4] = lvs[3].ToString("0");
            json.Values[NUMBER_5] = lvs[4].ToString("0");
            json.Values[NUMBER_6] = lvs[5].ToString("0");

            //GameData.SetLvs(lvs);

            Save(json);
        }
    }
}
