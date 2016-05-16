using Manatee.Trello;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloOrganizationRepository : TrelloRepository<TrelloOrganization, Organization>
    {
        public TrelloOrganizationRepository(PXGraph graph) : base(graph) { }

        public override IEnumerable<Organization> GetAllTrelloObject()
        {
            foreach (var organization in base.Member.Organizations)
            {
                yield return organization;
            }
        }

        public override Organization GetTrelloObjectByID(string id)
        {
            return new Organization(id);
        }

        public override TrelloOrganization To(Organization obj)
        {
            return new TrelloOrganization()
            {
                Id = obj.Id,
                Name = obj.Name,
                Descr = obj.Description
            };
        }
    }
}
