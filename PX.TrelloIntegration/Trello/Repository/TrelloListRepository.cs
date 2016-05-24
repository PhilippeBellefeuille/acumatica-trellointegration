using Manatee.Trello;
using System.Collections.Generic;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloListRepository : TrelloRepository<TrelloList, List>
    {
        public TrelloListRepository(object arg) : base(arg) { }

        public override IEnumerable<List> GetAllTrelloObject(string parentID = null)
        {
            if(!string.IsNullOrEmpty(parentID))
            {
                var board = new Board(parentID);
                if(board != null)
                {
                    foreach(var list in board.Lists)
                    {
                        yield return list;
                    }
                }
            }
        }

        public override List GetTrelloObjectByID(string id)
        {
            return new List(id);
        }

        public override TrelloList To(List list)
        {
            return new TrelloList
            {
                Id = list.Id,
                Name = list.Name
            };
        }
    }
}
