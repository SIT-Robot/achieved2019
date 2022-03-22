namespace SITRobotSystem_wpf.entity
{
    public class PoseStamped
    {
        public Header header;
        public Point position;
        public Quaternion oritation;

        public PoseStamped()
        {
            header = new Header();
            position = new Point();
            oritation = new Quaternion();
        }

        public PoseStamped(Header header, Point position, Quaternion oritation)
        {
            this.header = header;
            this.position = position;
            this.oritation = oritation;
        }
    }
}
