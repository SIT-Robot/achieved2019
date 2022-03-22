namespace SITRobotSystem_wpf.entity
{
    public class ObjCameraPosition
    {
        public float X;
        public float Y;

        public float Z;

        public ObjCameraPosition(float x,float y,float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public ObjCameraPosition()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

    }
}
