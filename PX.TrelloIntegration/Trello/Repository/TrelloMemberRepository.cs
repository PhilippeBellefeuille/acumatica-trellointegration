using Manatee.Trello;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloMemberRepository : TrelloRepository<TrelloMember, Member>
    {
        public TrelloMemberRepository(PXGraph graph) : base(graph) { }
        public TrelloMemberRepository(TrelloSetup setup) : base(setup) { }

        public TrelloMember GetCurrentMember()
        {
            return To(base.Member);
        }

        public override IEnumerable<Member> GetAllTrelloObject()
        {
            yield return base.Member;   
        }

        public override Member GetTrelloObjectByID(string id)
        {
            return new Member(id);
        }

        public override TrelloMember To(Member obj)
        {
            return new TrelloMember()
            {
                Id = obj.Id,
                Name = obj.UserName,
                Descr = obj.FullName
            };
        }
    }
}
