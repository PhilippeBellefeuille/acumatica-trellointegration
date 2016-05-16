using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.TrelloIntegration.Trello
{
    public interface ITrelloObject
    {
        string Id { get; set; }
    }

    public interface ITrelloOrganizationObject
    {
        string ParentId { get; set; }
    }
}
