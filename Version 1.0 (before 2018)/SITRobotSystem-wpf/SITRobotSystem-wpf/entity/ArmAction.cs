namespace SITRobotSystem_wpf.entity
{
    public class ArmAction
    {
        public int ID;
        public string actionStrs;

        public ArmAction()
        {
        }

        public ArmAction(int id, string actionStrs)
        {
            ID = id;
            this.actionStrs = actionStrs;
        }
    }
}
