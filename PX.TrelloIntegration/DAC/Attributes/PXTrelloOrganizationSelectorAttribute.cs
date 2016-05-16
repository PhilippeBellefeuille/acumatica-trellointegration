using PX.Data;
using System;
using System.Collections.Generic;
using System.Collections;
using Manatee.Trello.ManateeJson;
using Manatee.Trello;
using Manatee.Trello.RestSharp;
using PX.TrelloIntegration.Trello;

namespace PX.TrelloIntegration
{
    public class PXTrelloOrganizationSelectorAttribute : PXCustomSelectorAttribute
    {
        public PXTrelloOrganizationSelectorAttribute() : base(typeof(TrelloOrganization.id)) { }

        public IEnumerable GetRecords()
        {
            return new TrelloOrganizationRepository(_Graph).GetAll();
        }
    }
}
