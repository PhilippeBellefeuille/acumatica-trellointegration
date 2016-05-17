using PX.Data;

namespace PX.TrelloIntegration
{
    public class TrelloBoardMappingMaint : PXGraph<TrelloBoardMappingMaint, TrelloBoardMapping>
    {
        public PXSelect<TrelloBoardMapping> Board;
    }
}