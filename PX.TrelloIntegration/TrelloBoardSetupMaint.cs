using PX.Data;

namespace PX.TrelloIntegration
{
    public class TrelloBoardSetupMaint : PXGraph<TrelloBoardSetupMaint, TrelloBoardSetup>
    {
        public PXSelect<TrelloBoardSetup> Board;
    }
}