using PX.Data;

namespace PX.TrelloIntegration
{
    public class caseScreenID : Constant<string>
    {
        public caseScreenID()
            : base("CR306000")
        {
        }
    }

    public enum BoardTypes
    {
        Lead = 1,
        Case = 2
    }
}
