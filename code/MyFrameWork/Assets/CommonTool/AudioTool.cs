
namespace CommonTool
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class AudioTool : SingletonMode<AudioTool>
    {

        AudioSource BgAudio;        //背景音频
        AudioSource Audio;          //当前音频

        List<AudioSource> audioList;    //音频集合

        ResourceTool GameResouce;

        protected override void Awake()
        {
            base.Awake();
            GameResouce = ResourceTool.Instance;

            BgAudio = gameObject.AddComponent<AudioSource>();
            BgAudio.playOnAwake = false;
            BgAudio.loop = true;

            Audio = gameObject.AddComponent<AudioSource>();
        }

        //播放背景音乐
        public void PlayBGAudio(string audioName)
        {
            //加载资源
            AudioClip clip = GameResouce.GetAudio(audioName);
            //播放
            if (clip != null)
            {
                BgAudio.clip = clip;
                BgAudio.Play();
            }
            else
            {
                Debug.Log("资源池中找不到音频" + audioName);
            }
        }
        //关闭背景音乐
        public void CloseBGAudio()
        {
            BgAudio.Stop();
        }
        //暂停背景音乐
        public void PauseBGAudio()
        {
            BgAudio.Pause();
        }


        /// <summary>
        /// 音效播放（直接切换）
        /// </summary>
        /// <param name="audioName"></param>
        public void PlayAudio(string audioName)
        {
            //加载资源
            AudioClip clip = GameResouce.GetAudio(audioName);
            if(clip == null)
            {
                Debug.Log("找不到该音频" + audioName);
                return;
            }
            Audio.clip = clip;
            //播放
            Audio.Play();
        }


        /*
            实现不覆盖音乐处理，最好不要多生成对象
             */

        /// <summary>
        /// 音效播放（判断有无）
        /// </summary>
        /// <param name="audioName"></param>
        //public void PlayBuffAudio(string audioName)
        //{
        //    //加载资源
        //    AudioClip tempClip = GameResouce.GetAudio(audioName);
        //    if (tempClip == null)
        //    {
        //        Debug.Log("找不到该音频" + audioName);
        //        return;
        //    }
        //    if (Audio.isPlaying)
        //    {
        //        AudioSource temp = null;

        //        //list无对象时
        //        if (audioList == null)
        //        {
        //            temp = gameObject.AddComponent<AudioSource>();
        //            temp.clip = tempClip;

        //            audioList = new List<AudioSource>();
        //            audioList.Add(temp);
        //            temp.Play();
        //        }
        //        else
        //        {
        //            for (int i = 0; i < audioList.Count; i++)
        //            {
        //                if (!audioList[i].isPlaying)
        //                {
        //                    temp = audioList[i];
        //                    temp.Play();
        //                    break;
        //                }
        //            }
        //            if(temp == null)
        //            {
        //                temp = gameObject.AddComponent<AudioSource>();
        //                temp.clip = tempClip;

        //                audioList.Add(temp);
        //                temp.Play();
        //                return;
        //            }
        //        } 
        //    }
        //    else
        //    {
        //        Audio.clip = tempClip;
        //        //播放
        //        Audio.Play();
        //    }
        //}

        /// <summary>
        /// 关闭音乐
        /// </summary>
        public void CloseAudio()
        {
            Audio.Stop();
        }
        /// <summary>
        /// 声音暂停
        /// </summary>
        public void PauseAudio()
        {
            Audio.Pause();
        }
    }

}
