using UnityEngine;
using System.Collections;
using CommonTool.GameStatus;
using CommonTool;

/// <summary>
/// 状态字段
/// </summary>
public static class StatusConst
{
    public const string Start = "start";
    public const string Stay = "stay";
    public const string Leave = "leave";

}

/// <summary>
/// 状态机测试
/// </summary>
public class TestStatus : MonoBehaviour
{
    public ActionTool OnStartEntry = EventTool.CreateEvent();
    public ActionTool OnStartUpdate = EventTool.CreateEvent();
    public ActionTool OnStartLeave = EventTool.CreateEvent();

    public ActionTool OnStayEntry = EventTool.CreateEvent();
    public ActionTool OnStayUpdate = EventTool.CreateEvent();
    public ActionTool OnStayLeave = EventTool.CreateEvent();

    public ActionTool OnLeaveEntry = EventTool.CreateEvent();
    public ActionTool OnLeaveUpdate = EventTool.CreateEvent();
    public ActionTool OnLeaveLeave = EventTool.CreateEvent();

    public const int Timer1 = 0;
    public const int Timer2 = 1;
    public const int Timer3 = 2;
    public const int Timer4 = 3;

    public const int Length = 5;



    private StatusManager statusManager;

    private void Start()
    {
        statusManager = new StatusManager();

        StartStatus Start = new StartStatus(statusManager, StatusConst.Start);
        StayStatus Stay = new StayStatus(statusManager, StatusConst.Stay);
        LeaveStatus Leave = new LeaveStatus(statusManager, StatusConst.Leave);

        statusManager.InitTimer(Length);

        statusManager.AddStatus(Start);
        statusManager.AddStatus(Stay);
        statusManager.AddStatus(Leave);
        statusManager.InitStatus(Start);

    }

    private void Update()
    {
        statusManager.UpdateSystem();
    }


    public class StartStatus : StatusBase
    {
        public StartStatus(StatusManager _statusSystem, string _statusName) : base(_statusSystem, _statusName)
        {
        }



        public override void OnEntry(object param)
        {
            Debug.Log("-->start");

            TimerTool.OpenOnce(10.0f, () =>
            {
                Debug.Log("计时器结束");
            });

            TimerTool.OpenLoop(1.0f, -1, () =>
            {
                Debug.Log("调用一次响应一次");
            });
        }

        public override void OnLeave(object param)
        {
            Debug.Log("start Leave");
        }

        float testTime = 0;
        float testTime2 = 0;
        float testTime3 = 0;

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StatusHandle.TimerOpenOnce(Timer1, 2.0f, () =>
                {
                    Debug.Log("Timer1 Over");
                });
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                StatusHandle.TimerOpenLoop(Timer2, 1.0f, -1, () =>
                {
                    testTime++;
                    Debug.Log(testTime + "...");
                });
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                StatusHandle.TimerOpenLoop(Timer3, 1.0f, -1, () =>
                {
                    testTime++;
                    Debug.Log(testTime2 + "...2");
                });
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                StatusHandle.TimerOpenLoop(Timer4, 1.0f, -1, () =>
                {
                    testTime++;
                    Debug.Log(testTime3 + "...3");
                });
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                StatusHandle.CloseTimer(Timer3);
            }




        }
    }

    public class StayStatus : StatusBase
    {
        public StayStatus(StatusManager _statusSystem, string _statusName) : base(_statusSystem, _statusName)
        {
        }

        public override void OnEntry(object param)
        {
            Debug.Log("-->Stay");
        }

        public override void OnLeave(object param)
        {
           
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StatusHandle.ChangeStatus(StatusConst.Leave);
            }
        }
    }

    public class LeaveStatus : StatusBase
    {
        public LeaveStatus(StatusManager _statusSystem, string _statusName) : base(_statusSystem, _statusName)
        {
        }

        public override void OnEntry(object param)
        {
            Debug.Log("-->Leave");
        }

        public override void OnLeave(object param)
        {
            
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StatusHandle.ChangeStatus(StatusConst.Start);
            }
        }
    }



}
