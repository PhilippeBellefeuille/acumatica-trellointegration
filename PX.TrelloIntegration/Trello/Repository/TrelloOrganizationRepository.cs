using Manatee.Trello;
using System.Collections.Generic;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloOrganizationRepository : TrelloRepository<TrelloOrganization, Organization>
    {
        public TrelloOrganizationRepository(object arg) : base(arg) { }

        public override IEnumerable<Organization> GetAllTrelloObject(string parentID = null)
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
