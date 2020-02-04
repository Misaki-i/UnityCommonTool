
namespace CommonTool
{
    using UnityEngine;

    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class SingletonMode<T> : MonoBehaviour where T : MonoBehaviour
    {

        /// <summary>
        /// 外部拖拉 确定好全局的对象
        /// </summary>
        public T InstanceTarget;

        private static T instance;

        protected virtual void Awake()
        {
            instance = InstanceTarget;
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                        Debug.Log("找不到此单例");
                }
                return instance;
            }
        }

        private void OnDestroy()
        {
            instance = default(T);
        }
    }


}
