namespace SITRobotSystem_wpf.entity
{
    public class EulerAngle
    {
        public EulerAngle()
        {
        }

        public EulerAngle(double fYaw, double fPitch, double fRoll)
        {
            _fYaw = fYaw;
            _fPitch = fPitch;
            _fRoll = fRoll;
        }

        private double _fYaw;
        private double _fPitch;
        private double _fRoll;

        public double FYaw
        {
            get { return _fYaw; }
            set { _fYaw = value; }
        }

        public double FPitch
        {
            get { return _fPitch; }
            set { _fPitch = value; }
        }

        public double FRoll
        {
            get { return _fRoll; }
            set { _fRoll = value; }
        }
    }
}
