/****************************************************
    文件: ControllerMode.cs
	作者: Misakiか
    邮箱: 1016181268@qq.com
    日期: 2019/8/11 19:56:26
	功能：控制模板参考
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.CommonTool.MyMode
{
    public enum InputDirection
    {
        NULL,
        Right,
        Left,
        Down,
        Up
    }

    public class ControllerMode:MonoBehaviour
    {
        #region 玩家移动控制方法
        //获取用户输入(手势、键盘)
        //Warning左右连续按会有Bug

        private CharacterController player;
        private InputDirection InputDir = InputDirection.NULL;
        private bool activeInput = false;
        private Vector3 mousePos;

        //移动变量
        private int nowIndex = 1;
        private int targetIndex = 1;

        private float xDistance;
        private float yDistance;

        //Roll变量
        private bool isSlide;
        private float SlideTime;

        //获取输入方法
        void GetInputDirection()
        {
            //手势方向输入
            InputDir = InputDirection.NULL;
            if (Input.GetMouseButtonDown(0))
            {
                activeInput = true;
                mousePos = Input.mousePosition;
            }

            //鼠标或手指输入
            if (Input.GetMouseButton(0) && activeInput)
            {
                Vector3 Dir = Input.mousePosition - mousePos;
                if (Dir.magnitude > 40)  //限制 : >20才判断
                {
                    if (Mathf.Abs(Dir.x) > Mathf.Abs(Dir.y) && Dir.x > 0)
                    {
                        InputDir = InputDirection.Right;
                    }
                    else if (Mathf.Abs(Dir.x) > Mathf.Abs(Dir.y) && Dir.x < 0)
                    {
                        InputDir = InputDirection.Left;
                    }
                    else if (Mathf.Abs(Dir.x) < Mathf.Abs(Dir.y) && Dir.y > 0)
                    {
                        InputDir = InputDirection.Up;
                    }
                    else if (Mathf.Abs(Dir.x) < Mathf.Abs(Dir.y) && Dir.y < 0)
                    {
                        InputDir = InputDirection.Down;
                    }
                    activeInput = false;

                }
            }

            //键盘输入
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                InputDir = InputDirection.Up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                InputDir = InputDirection.Down;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                InputDir = InputDirection.Left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                InputDir = InputDirection.Right;
            }
            //print(m_inputDir);

        }

        //更新位置方法
        void UpdatePosition()
        {
            GetInputDirection();
            //判断
            switch (InputDir)
            {
                case InputDirection.NULL:
                    break;
                case InputDirection.Right:
                    if (targetIndex < 2)
                    {
                        targetIndex++;
                        xDistance = 2;
                        //SendMessage("AnimManager", InputDir);     //动画播放

                    }

                    break;
                case InputDirection.Left:
                    if (targetIndex > 0)
                    {
                        targetIndex--;
                        xDistance = -2;
                        //SendMessage("AnimManager", InputDir);

                    }
                    break;
                case InputDirection.Down:
                    if (isSlide == false)
                    {
                        isSlide = true;
                        SlideTime = 0.7f;
                        //SendMessage("AnimManager", InputDir);

                    }

                    break;
                case InputDirection.Up:
                    if (player.isGrounded)
                    {
                        yDistance = JumpValue;
                        //SendMessage("AnimManager", InputDir);

                    }
                    break;

            }

        }

        float MoveSpeed = 10;
        float JumpValue = 5;

        //位置移动控制
        void MoveController()
        {
            //左右移动
            if (nowIndex != targetIndex)
            {
                float move = Mathf.Lerp(0, xDistance, MoveSpeed * Time.deltaTime);
                transform.position += new Vector3(move, 0, 0);
                xDistance -= move;
                if (Mathf.Abs(xDistance) < 0.05f)
                {
                    xDistance = 0;
                    nowIndex = targetIndex;
                    //强制转换
                    switch (nowIndex)
                    {
                        case 0:
                            transform.position = new Vector3(-2, transform.position.y, transform.position.z);
                            break;
                        case 1:
                            transform.position = new Vector3(0, transform.position.y, transform.position.z);
                            break;
                        case 2:
                            transform.position = new Vector3(2, transform.position.y, transform.position.z);
                            break;
                    }

                }
            }

            //计时器
            if (isSlide)
            {
                SlideTime -= Time.deltaTime;
                if (SlideTime <= 0)
                {
                    isSlide = false;
                    SlideTime = 0;
                }
            }


        }
        #endregion



    }
}
