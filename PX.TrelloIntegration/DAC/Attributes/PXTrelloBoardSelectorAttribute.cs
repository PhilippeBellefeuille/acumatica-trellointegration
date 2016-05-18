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
    public class PXTrelloBoardSelectorAttribute : PXTrelloSelectorBaseAttribute
    {
        public override Type RepositoryType
        {
            get
            {
                return typeof(TrelloBoardRepository);
            }
        }

        public PXTrelloBoardSelectorAttribute() : base(typeof(TrelloBoard.id))
        {
            DescriptionField = typeof(TrelloBoard.name);
        }
        
        public override IEnumerable GetTrelloRecords()
        {
            return ((TrelloBoardRepository)Repository).GetAll();
        }
    }
}
