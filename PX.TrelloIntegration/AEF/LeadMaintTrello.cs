using PX.Data;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
    public class LeadMaintTrello : PXGraphExtension<LeadMaint>
    {
        public CRTrelloSelect<Contact, Contact.noteID, Contact.classID> Trello;
    }
}
