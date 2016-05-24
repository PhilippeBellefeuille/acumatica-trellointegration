using PX.Data;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
    public class CRCaseMaintTrello : PXGraphExtension<CRCaseMaint>
    {
        public CRTrelloSelect<CRCase.noteID> Trello;
    }
}
