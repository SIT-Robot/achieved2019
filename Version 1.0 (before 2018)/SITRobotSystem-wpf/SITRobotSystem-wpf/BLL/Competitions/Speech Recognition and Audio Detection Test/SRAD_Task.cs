using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.Tasks;
using SITRobotSystem_wpf.entity;
using SITRobotSystem_wpf.BLL.ServiceCtrl;

using System.Threading;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SITRobotSystem_wpf.BLL.Competitions.Speech_Recognition_and_Audio_Detection_Test
{
    class SRAD_Task : Tasks.Tasks
    {

        SqlConnection Connection2015 = new SqlConnection();
        SqlCommand Command2015 = new SqlCommand();

        public SurfResult[] peoples;

        
        /// <summary>
        /// 获取机器人面前的人数(人脸)
        /// </summary>
        public int getfaceNum()
        {
            int maxNum = 0;
            for (int i = 0; i < 7; i++)
            {
                peoples = this.CountFace();
                if (peoples.Count() > maxNum)
                    maxNum = peoples.Count();
            }
            return maxNum;
        }

        /// <summary>
        /// 获取机器人面前的人数（骨架）
        /// </summary>
        public int getAllUser()
        {
            List<User> users;
            int max = 0;
            for(int i=0;i<5;i++)
            {
                Thread.Sleep(500);
                users = visionCtrl.getAllusers();
                if (users.Count > max)
                    max = users.Count;
            }
            return max;
        }

        public void showSRADRecognized()
        {
           // this.baseSpeech .SRADRecognize_ver2();
            while (this.baseSpeech.RecognitedCount <8)
            {
                System.Threading.Thread.Sleep(1000);
            }
            System.Threading.Thread.Sleep(3000);
            this.baseSpeech.robotSpeak("It is end");
        }

        //follow的部分功能
        //public int TrackedPeopleID = 0;

        ///// 通过手势的形式记住人
        //public int RememberUserByHandState()
        //{
        //    List<User> users = getAllUser();
        //    User handOnUser = findUserMiddleRasingHand(users);
        //    return handOnUser.ID;
        //}

        //public User findUser(int userId)
        //{
        //    User user = findCorrectUser(userId);
        //    return user;
        //}



    }
}
