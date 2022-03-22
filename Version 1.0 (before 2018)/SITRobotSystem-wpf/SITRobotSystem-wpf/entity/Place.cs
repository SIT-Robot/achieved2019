namespace SITRobotSystem_wpf.entity
{
    public class Place : Thing
    {

        public Header header;
        public Point position;
        public Quaternion oritation;
        public string PCategory;
        public string ProomName;
        public Place()
        {
            header = new Header();
            position = new Point();
            oritation = new Quaternion();
        }

        public Place(string no,string name,string proomName, string pCategory, Quaternion oritation, Point position, Header header)
        {
            No = no;
            Name = name;
            ProomName = proomName;
            PCategory = pCategory;
            this.oritation = oritation;
            this.position = position;
            this.header = header;
        }
        /// <summary>
        /// BUG!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="poseStamped"></param>
        /// <returns></returns>
        public static Place change(PoseStamped poseStamped)
        {
            Quaternion orientation = new Quaternion(poseStamped.oritation.X, poseStamped.oritation.Y,
                poseStamped.oritation.Z, poseStamped.oritation.W);
            Point point=new Point(poseStamped.position.X,poseStamped.position.Y,poseStamped.position.Z);
            Header header = new Header(poseStamped.header.PlaceName, poseStamped.header.FrameId);
            Place place=new Place(header.PlaceName,header.PlaceName,"NULL","NULL",orientation,point,header);
            return place;
        }
    }
}
