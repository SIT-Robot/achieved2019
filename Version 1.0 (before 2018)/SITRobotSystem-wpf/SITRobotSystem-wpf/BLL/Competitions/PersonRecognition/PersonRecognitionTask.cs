using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SITRobotSystem_wpf.BLL.ServiceCtrl;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.PersonRecognition
{
    class PersonRecognitionTask:Tasks.Tasks
    {
        
        public bool trainsign = false;
        public SurfResult[] MemorizePeople;
        public SurfResult[] peoples;
        public bool turnsign = false;
        private string oprname;
        public bool Memorize()
        {
            oprname="";
                this.baseSpeech = new SitRobotSpeech();
                this.speak("May I know your name?");
                Thread.Sleep(100);
                this.baseSpeech.PersonNameSpeechRecognize();
            while(string.IsNullOrWhiteSpace(oprname))
            {
                oprname = baseSpeech.ReturnCommand;
            }

            MemorizePeople = this.CountFace();
            //this.speak("train ok");
            //Thread.Sleep(1000);
            //this.visionCtrl.FaceRecognitionTrain(oprname);
            trainsign = true;
            return trainsign;
        }

        private string res;
        public bool Wait()
        {
            res = "";
            this.baseSpeech = new SitRobotSpeech();
            this.speak("Are you ok?");
            Thread.Sleep(1000);
            this.baseSpeech.PersonSpeechRecognize();
            while (string.IsNullOrWhiteSpace(res))
            {
                res = baseSpeech.ReturnCommand;
            }

            this.baseCtrl.moveToDirectionSpeedW(3.13f);
            //this.speak("turn 180");
            Thread.Sleep(100);
            turnsign = true;

            return turnsign;
        }
        public bool Approach()
        {
            return true;
        }
        public bool LookFor()
        {
            return true;
        }
        public bool PoseRecogniton()
        {
            return true;
        }
        public Sex GenderRecogniton()
        {
            List<User> users= getAllUser();
            User userbuf =new User();
            foreach (var user in users)
            {
                userbuf = visionCtrl.findUserSex(user);
                Console.WriteLine(user.ID + " sex--->" + userbuf.sex.ToString() + "--breastSize:" + userbuf.breastsize);
            }
            return userbuf.sex;
        }
        public bool AgeRecognition()
        {
            return true;
        }
        public bool InitCrows()
        {
            CrowSize = new List<string>();
            return true;
        }
        public bool StateGender(int gender)
        {
            return true;
        }
        public bool StatePose(int pose)
        {
            return true;
        }

        
        public string size = null;
        public List<string> CrowSize;
        public bool StateSize()
        {
            int cmax = 0;
            int pos = 0;
            for (int i = 0; i < CrowSize.Count; i++)
            {
                int count = 0;
                for (int j = 0; j < CrowSize.Count; j++)
                {
                    if (CrowSize[j] == CrowSize[i])
                        count++;
                }
                if (count > cmax)
                {
                    cmax = count;
                    pos = i;
                }
            }
            this.speak("there are "+CrowSize[pos]+"people");
            return true;
        }
        public bool RecSize()
        {
            if (string.IsNullOrWhiteSpace(size))
                size = FaceNum();
            else
            {
                char[] cr = size.ToCharArray();
                StringBuilder strbuilder = new StringBuilder();
                foreach (char aa in cr)
                {
                    if ((aa >= '0' && aa <= '9'))
                    {
                        strbuilder.Append(aa);
                    }
                }
                size = strbuilder.ToString();
                CrowSize.Add(size);
                //this.speak(size);
                size = "";
            }
            return true;
        }
        public string FaceNum()
        {
            string s = visionCtrl.GetFaceNum();
            return s;
        }

        /// <summary>
        /// 获取机器人面前的人数
        /// </summary>
        public int getfaceNum()
        {
            int maxNum=0;
            for (int i = 0; i < 5; i++)
            {
                peoples = this.CountFace();
                if (peoples.Count() > maxNum)
                    maxNum = peoples.Count();
            }
            return maxNum;
        }
        public int LegNum()
        {
            int s = baseCtrl.PeopleNum(4);
            return s;
        }
    }
}
