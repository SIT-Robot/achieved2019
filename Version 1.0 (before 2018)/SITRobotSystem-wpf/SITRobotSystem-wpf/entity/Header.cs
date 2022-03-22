namespace SITRobotSystem_wpf.entity
{
    public class Header
    {
        private string _placename;
        private string _frameId;

        public Header(string placename, string frameId)
        {
            _placename = placename;
            _frameId = frameId;
        }

        public Header()
        {
        }

        public string PlaceName
        {
            set { _placename = value; }
            get { return _placename; }
        }

        public string FrameId
        {
            get { return _frameId; }
            set { _frameId = value; }
        }
    }
}
