using SITRobotSystem_wpf.BLL.FunctionClass;
using SITRobotSystem_wpf.entity;

namespace SITRobotSystem_wpf.BLL.Competitions.shopping
{
    class ShoppingCompetition:Competition
    {
        private ShoppingStage shoppingStage;
        private ShoppingStageFollow shoppingStageFollow;
        Place[] place = new Place[5];
        string[] objects = new string[] { "bottle yellow", "bottle blue", "bottle red", "drinks", "cashier desk" };
        string[] direction = new string[4];
        //int[] mark = new int[6];

        public ShoppingCompetition()
        { 
            shoppingStageFollow = new ShoppingStageFollow(objects);            
            shoppingStage = new ShoppingStage(objects, direction, place);
        }

        public override void init()
        {
            ThreadNameStr = "shopping";
            shoppingStage.init();
            shoppingStageFollow.init();
            
        }

        public override void process()
        {
            //place[place.Length-1]=shoppingStageFollow.task.GetPlace();
            User followUser = new User();
            Leg leg = new Leg();
            shoppingStageFollow.ensureUser(ref followUser, ref leg);
            shoppingStageFollow.followTrackingUser(ref followUser, ref leg);
            
            for (int i=0;i<shoppingStageFollow.place.Length;i++)
            {
                place[i] = shoppingStageFollow.place[i];
            }
            
            for (int k = 0; k < shoppingStageFollow.direction.Length; k++)
            {
                direction[k] = shoppingStageFollow.direction[k];
            }

            shoppingStage.place = place;
            shoppingStage.objects = objects;
            shoppingStage.direction = direction;

            string[] things = new[] {"toothpaste","coffee","chips","tea","juice","biscuit","rollpaper","sprite","redbull","milk"};
            shoppingStage.ShoppingCommand(things);

        }


        /*
        public string[] objects;
        public string[] directions;
        public Place[] places;

        public ShoppingCompetition()
        {
            
        }
        public void start()
        {
            stage1();

            stage2();
        }

        private void stage1()
        {
            ShoppingStageRemember sr = new ShoppingStageRemember();



            this.objects = sr.objects;
            this.directions = sr.directions;
            this.places = sr.places;
        }

        private void stage2()
        {
            ShoppingStageCommand sc = new ShoppingStageCommand(this.places,this.objects,this.directions);
            //sc.Start();
        }

        public override void init()
        {
            throw new System.NotImplementedException();
        }

        public override void process()
        {
            throw new System.NotImplementedException();
        }
         * */

    }
}
