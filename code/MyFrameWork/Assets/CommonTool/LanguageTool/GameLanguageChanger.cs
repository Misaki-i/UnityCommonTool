/****************************************************
    文件: GameLanguageChanger.cs
	作者: Misakiか
    邮箱: 1016181268@qq.com
    日期: 2019/8/11 20:37:24
	功能：游戏中英切换模块
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Assets.CommonTool.LanguageTool
{
    public class GameLanguageChanger :MonoBehaviour
    {
        public enum Language
        {
            Chs, Eng
        }
        public enum Types
        {
            Obj,
            Img,
            Txt,
        }
        private static Language language = Language.Chs;

        public static void SetLanguage(Language lan)
        {
            language = lan;
        }

        public Transform Chs;
        public Transform Eng;

        public Types Type = Types.Obj;
        public Sprite ChImg;
        public Sprite EnImg;

        public string ChTxt;
        public string EnTxt;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            if (Chs == null)
            {
                Chs = transform;
            }
            if (Eng == null)
            {
                Eng = transform;
            }
            switch (Type)
            {
                case Types.Obj:
                    if (language == Language.Chs)
                    {
                        Chs.gameObject.SetActive(true);
                        Eng.gameObject.SetActive(false);
                    }
                    else
                    {
                        Eng.gameObject.SetActive(true);
                        Chs.gameObject.SetActive(false);
                    }
                    break;

                case Types.Img:
                    if (language == Language.Chs)
                    {
                        Eng.gameObject.SetActive(false);
                        Chs.gameObject.SetActive(true);
                        Chs.GetComponent<Image>().sprite = ChImg;
                        Chs.GetComponent<Image>().SetNativeSize();
                    }
                    else
                    {
                        Chs.gameObject.SetActive(false);
                        Eng.gameObject.SetActive(true);
                        Eng.GetComponent<Image>().sprite = EnImg;
                        Eng.GetComponent<Image>().SetNativeSize();
                    }
                    break;

                case Types.Txt:
                    if (language == Language.Chs)
                    {
                        Eng.gameObject.SetActive(false);
                        Chs.gameObject.SetActive(true);
                        Chs.GetComponent<Text>().text = ChTxt;
                    }
                    else
                    {
                        Chs.gameObject.SetActive(false);
                        Eng.gameObject.SetActive(true);
                        Eng.GetComponent<Text>().text = EnTxt;
                    }
                    break;
            }
        }
    }
}
