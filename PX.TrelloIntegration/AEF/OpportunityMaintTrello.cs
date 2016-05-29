using PX.Data;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
    public class OpportunityMaintTrello : PXGraphExtension<OpportunityMaint>
    {
        public CRTrelloSelect<CROpportunity, CROpportunity.noteID, CROpportunity.cROpportunityClassID> Trello;
    }
}
