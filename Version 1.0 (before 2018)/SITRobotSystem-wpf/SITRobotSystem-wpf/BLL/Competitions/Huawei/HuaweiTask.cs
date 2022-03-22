using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.DAL;

using SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows;
using SITRobotSystem_wpf.SITRobotWindow.MainWindows;
using System.Windows;
using SITRobotSystem_wpf.SITRobotWindow.CompetitionWindows;

namespace SITRobotSystem_wpf.BLL.Competitions.Huawei
{
    public delegate void Start_Video_Del(string command);
    public class HuaweiTask:Tasks.Tasks
    {
        bool IsEnded = false;
        //定义事件
        public event Start_Video_Del Video_Event;
        public void VideoPlay()
        {
            Video_Event("start");
        }

        public void RightTurn()
        {
            moveDirectionBySpeedW(90);
        }

        public void LeftTurn()
        {
            moveDirectionBySpeedW(-90);
        }

        public void Ended_Handler(object sender, RoutedEventArgs e)
        {
            this.IsEnded = true;
        }

        public void Loop()
        {
            while (!this.IsEnded)
            { }
            this.IsEnded = false;
        }
    }
}