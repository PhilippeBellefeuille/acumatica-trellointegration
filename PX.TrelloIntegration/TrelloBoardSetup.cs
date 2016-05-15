using PX.Data;

namespace PX.TrelloIntegration
{
    public class TrelloBoardSetup : PXGraph<TrelloBoardSetup, TrelloBoard>
    {
        public PXSelect<TrelloBoard> Board;
    }
}