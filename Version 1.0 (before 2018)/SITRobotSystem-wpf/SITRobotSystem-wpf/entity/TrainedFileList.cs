using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SITRobotSystem_wpf.entity
{
    /// <summary>
    /// save all face sample
    /// </summary>
    public class TrainedFileList
    {
        /// <summary>
        /// image list
        /// </summary>
        public List<Image<Gray, byte>> trainedImages = new List<Image<Gray, byte>>();
        /// <summary>
        /// people number of the face in image list
        /// </summary>
        public List<int> trainedLabelOrder = new List<int>();
        /// <summary>
        /// people name of the face in image list
        /// </summary>
        public List<string> trainedFileName = new List<string>();

    }
}
