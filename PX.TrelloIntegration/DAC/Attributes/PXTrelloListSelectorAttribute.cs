using System;
using System.Collections;
using PX.TrelloIntegration.Trello;
using PX.Data;

namespace PX.TrelloIntegration
{
    public class PXTrelloListSelectorAttribute : PXTrelloSelectorBaseAttribute
    {
        public override Type RepositoryType
        {
            get
            {
                return typeof(TrelloListRepository);
            }
        }

        private Type _BoardID;

        public PXTrelloListSelectorAttribute(Type boardID) : base(typeof(TrelloList.id))
        {
            _BoardID = boardID;
            DescriptionField = typeof(TrelloList.name);
        }
        
        public override IEnumerable GetTrelloRecords()
        {
            return ((TrelloListRepository)Repository).GetAll(GetTrelloBoardID());
        }

        private string GetTrelloBoardID()
        {
            var cache = _Graph.Caches[_BqlTable];
            var boardID = (int?)cache.GetValue(cache.Current, _BoardID.Name);

            if (boardID == null)
            {
                return null;
            }
            else
            {
                var boardMapping = (TrelloBoardMapping)PXSelect<TrelloBoardMapping,
                                                            Where<TrelloBoardMapping.boardID, 
                                                                Equal<Required<TrelloBoardMapping.boardID>>>>
                                                            .Select(_Graph, boardID);
                if(boardMapping == null)
                {
                    return null;
                }
                else
                {
                    return boardMapping.TrelloBoardID;
                }
            }
        }
    }
}
