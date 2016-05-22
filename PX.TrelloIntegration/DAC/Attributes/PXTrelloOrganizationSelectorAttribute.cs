using System;
using System.Collections;
using PX.TrelloIntegration.Trello;

namespace PX.TrelloIntegration
{
    public class PXTrelloOrganizationSelectorAttribute : PXTrelloSelectorBaseAttribute
    {
        public override Type RepositoryType
        {
            get
            {
                return typeof(TrelloOrganizationRepository);
            }
        }

        public PXTrelloOrganizationSelectorAttribute() : base(typeof(TrelloOrganization.id))
        {
            DescriptionField = typeof(TrelloOrganization.name);
        }
        
        public override IEnumerable GetTrelloRecords()
        {
            return ((TrelloOrganizationRepository)Repository).GetAll();
        }
    }
}
