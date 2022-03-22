namespace SITRobotSystem_wpf.entity
{
    public class Quaternion
    {
        private double _x;

        public Quaternion(double x, double y, double z, double w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        public Quaternion()
        {
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private double _z;

        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        private double _w;

        public double W
        {
            get { return _w; }
            set { _w = value; }
        }
    }
}
