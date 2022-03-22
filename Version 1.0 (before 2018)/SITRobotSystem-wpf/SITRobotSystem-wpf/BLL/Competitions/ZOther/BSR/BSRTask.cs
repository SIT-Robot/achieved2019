using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.BLL.Competitions.BSR
{
    class BSRTask:Tasks.Tasks
    {
        public void startDepthReader()
        {
            visionCtrl.startDepthReader();
            visionCtrl.SetDepthReaderRobotInfo(0.20f,0.5f,1.1f);
        }

        /// <summary>
        /// 返回门是否为打开状态
        /// </summary>
        /// <returns></returns>
        public bool IsdoorOpen()
        {
            bool res=true;
            int num=visionCtrl.getUnSafeCount(256);
            
            if (num > 50) res=false;
            
            return res;
        }

        public bool BSRmovetoPlaceByName(string name)
        {
            speak("I will go to "+name);
            bool res=this.moveToPlaceByName(name);
            if (res)
            {
                speak("I Have arrived at point " + name);
            }
            else
            {
                speak("I fail to arrive at point " + name);
            }
            //speak("let me sleep for 14 seconds");
            Thread.Sleep(14000);


            return res;
        }
    }
}
