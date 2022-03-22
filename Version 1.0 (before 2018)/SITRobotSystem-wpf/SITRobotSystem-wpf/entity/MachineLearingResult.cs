using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITRobotSystem_wpf.entity
{
    public class MachineLearingResult
    {
        string name;
        CameraSpacePoint[] cameraSpacePoint;
        int thingsNumber;
        float[] confidence;

        public CameraSpacePoint[] CameraSpacePoint
        {
            get
            {
                return cameraSpacePoint;
            }

            set
            {
                cameraSpacePoint = value;
            }
        }

        public int ThingsNumber
        {
            get
            {
                return thingsNumber;
            }

            set
            {
                thingsNumber = value;
            }
        }

        public float[] Confidence
        {
            get
            {
                return confidence;
            }

            set
            {
                confidence = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public MachineLearingResult(int incomingthingsnumber)
        {
            Name = "";
            this.cameraSpacePoint = new Microsoft.Kinect.CameraSpacePoint[incomingthingsnumber];
            this.thingsNumber = incomingthingsnumber;
            this.confidence = new float[incomingthingsnumber];
        }

        public MachineLearingResult(string name, CameraSpacePoint[] cameraSpacePoint, int thingsNumber, float[] confidence)
        {
            this.Name = name;
            this.cameraSpacePoint = cameraSpacePoint;
            this.thingsNumber = thingsNumber;
            this.confidence = confidence;
        }
    }
}
