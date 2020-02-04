
namespace CommonTool
{
    
    /// <summary>
    /// 委托声明
    /// </summary> 

    public delegate void ActionTool();
    public delegate void ActionTool<T1>(T1 obj);
    public delegate void ActionTool<T1, T2>(T1 obj1, T2 obj2);
    public delegate void ActionTool<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3);


    /// <summary>
    /// 游戏事件
    /// </summary>
    public struct EventTool
    {
        public static ActionTool CreateEvent()
        {
            ActionTool action = new ActionTool(() => { });
            return action;
        }
    }
    /// <summary>
    /// 游戏事件 参数1
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public struct EventTool<T1>
    {
        public static ActionTool<T1> CreateEvent()
        {
            ActionTool<T1> action = new ActionTool<T1>((t) => { });
            return action;
        }
    }
    /// <summary>
    /// 游戏事件 参数1 参数2
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public struct EventTool<T1,T2>
    {
        public static ActionTool<T1,T2> CreateEvent()
        {
            ActionTool<T1, T2> action = new ActionTool<T1, T2>((t1, t2) => { });
            return action;
        }
    }
    /// <summary>
    /// 游戏事件 参数1 参数2 参数3
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public struct EventTool<T1,T2,T3>
    {
        public static ActionTool<T1, T2, T3> CreateEvent()
        {
            ActionTool<T1, T2, T3> action = new ActionTool<T1, T2, T3>((t1, t2, t3) => { });
            return action;
        }
    }

    /*
     
      public event ActionTool OnPlayer = EventTool.CreatEvent();
     
     */

}
