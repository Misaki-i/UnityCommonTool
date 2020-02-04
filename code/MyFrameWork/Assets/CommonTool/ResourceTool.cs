
namespace CommonTool
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 可序列化用于在Inspector视图显示
    /// </summary>

    //音频信息
    [System.Serializable]
    public struct AudioDic
    {
        public string Key;
        public AudioClip Clip;
    }

    //GameObject信息
    [System.Serializable]
    public struct GameObjectDic
    {
        public string Key;
        public GameObject Target;
    }

    /// <summary>
    /// 游戏对象，音频存储
    /// </summary>
    public class ResourceTool : SingletonMode<ResourceTool>
    {
        //外部拖拽
        public AudioDic[] AudioDics;
        public GameObjectDic[] ObjectDics;

        //内部存储
        private Dictionary<string, GameObject> objects;
        private Dictionary<string, AudioClip> audios;

        /// <summary>
        /// 获取游戏对象
        /// </summary>
        /// <param name="key">对象key</param>
        /// <returns></returns>
        public GameObject GetObject(string key)
        {
            //第一次调用添加进objects字典中
            if (objects == null)
            {
                objects = new Dictionary<string, GameObject>();
                for (int i = 0; i < ObjectDics.Length; i++)
                {
                    objects.Add(ObjectDics[i].Key, ObjectDics[i].Target);
                }
            }
            if (objects[key] != null)
            {
                return objects[key];
            }
            else
            {
                Debug.Log("GameResource中不存在键为：" + key + "的对象！");
                return null;
            }
        }
        /// <summary>
        /// 获取游戏音频
        /// </summary>
        /// <param name="key">音频key</param>
        /// <returns></returns>
        public AudioClip GetAudio(string key)
        {
            if (audios == null)
            {
                audios = new Dictionary<string, AudioClip>();
                for (int i = 0; i < AudioDics.Length; i++)
                {
                    audios.Add(AudioDics[i].Key, AudioDics[i].Clip);
                }
            }
            if (audios[key] != null)
            {
                return audios[key];
            }
            else
            {
                Debug.Log("GameResource中不存在键为：" + key + "的音频！");
                return null;
            }
        }

    }
}

