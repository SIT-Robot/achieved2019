namespace SITRobotSystem_wpf.entity
{
    public enum ActionType
    {
        move =0,
        get=1,
        put=2,
        find=3,
        say=4,
        introduce=5,
        follow=6,
        tell=7,
        ask=8,
        count=9,
        give=10,
        back=11,
        lookfor=12,
        answeraquestion=13,
        tellyourname=14,
        tellthenameofyourteam=15,
        tellthetime=16,
        tellwhattimeisit=17,
        telltellthedate=18,
        tellwhatdayistoday=19,
        tellwhatdayistomorrow = 22,
        tellthedayofthemonth=20,
        tellthedayoftheweek = 21,
        back2=23,
    }

    public class Command
    {
        public ActionType action;
        public Thing thing;

        public Command(ActionType action, Thing thing)
        {
            this.action = action;
            this.thing = thing;
        }
    }
}
