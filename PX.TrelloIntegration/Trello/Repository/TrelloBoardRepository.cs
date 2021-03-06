﻿using Manatee.Trello;
using System.Collections.Generic;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloBoardRepository : TrelloOrganizationableRepository<TrelloBoard, Board>
    {
        public TrelloBoardRepository(object arg) : base(arg) { }

        public override IEnumerable<Board> GetAllTrelloObject(string parentID = null)
        {
            foreach (var board in BoardCollection)
            {
                yield return board;
            }
        }

        public override Board GetTrelloObjectByID(string id)
        {
            return new Board(id);
        }

        public override TrelloBoard To(Board board)
        {
            return new TrelloBoard
            {
                Id = board.Id,
                OrganizationId = board.Organization?.Id,
                Name = board.Name,
                Descr = board.Description
            };
        }
    }
}
