using PX.Data;

namespace PX.TrelloIntegration
{
    public class CRTrelloSelect : PXSelect<TrelloCardMapping>
    {
        public CRTrelloSelect(PXGraph graph) : base(graph) { }
    }
}
