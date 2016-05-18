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
        public TrelloMemberRepository(object arg) : base(arg) { }

        public TrelloMember GetCurrentMember()
        {
            return To(base.Member);
        }

        public override IEnumerable<Member> GetAllTrelloObject(string boardID)
        {
            foreach(var boardMember in new Board(boardID).Members)
            {
                yield return boardMember;
            }  
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
