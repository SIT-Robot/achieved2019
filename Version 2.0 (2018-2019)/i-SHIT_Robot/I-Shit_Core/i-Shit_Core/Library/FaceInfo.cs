using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace i_Shit_Core.Library {
    public class FaceInfo {
        /// <summary>
        /// 人脸的代号
        /// </summary>
        public int ID;  // 弃用
        /// <summary>
        /// 人脸的位置
        /// </summary>
        public Rectangle FaceLocation;
        public string Name;

        public int sex; // 1 male. 0 female.
    }
}