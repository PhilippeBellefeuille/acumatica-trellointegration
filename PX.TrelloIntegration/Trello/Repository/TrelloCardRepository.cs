using Manatee.Trello;
using System.Collections.Generic;
using System;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloCardRepository : TrelloUpdatableRepository<TrelloCard, Card>
    {
        public TrelloCardRepository(object arg) : base(arg) { }

        public override IEnumerable<Card> GetAllTrelloObject(string parentID = null)
        {
            if(!string.IsNullOrEmpty(parentID))
            {
                var board = new Board(parentID);
                if(board != null)
                {
                    foreach(var card in board.Cards)
                    {
                        yield return card;
                    }
                }
            }
        }

        public override Card GetTrelloObjectByID(string id)
        {
            return new Card(id);
        }
        
        public override TrelloCard To(Card card)
        {
            return new TrelloCard
            {
                Id = card.Id,
                ListId = card.List.Id,
                Name = card.Name,
                Description = card.Description
            };
        }

        public override string Insert(TrelloCard trelloCard)
        {
            var list = new List(trelloCard.ListId);
            if(list != null)
            {
                var newCard = list.Cards.Add(trelloCard.Name);
                newCard.Description = trelloCard.Description;
                return newCard.Id;
            }
            return string.Empty;
        }

        public override bool Update(TrelloCard trelloCard)
        {
            var card = new Card(trelloCard.Id);
            var list = new List(trelloCard.ListId);

            if(card != null && list != null)
            {
                card.List = list;
                card.Name = trelloCard.Name;
                card.Description = trelloCard.Description;
                return true;
            }
            return false;
        }
        
        public override bool Delete(TrelloCard trelloCard)
        {
            var card = new Card(trelloCard.Id);
            if (card != null)
            {
                card.Delete();
                return true;
            }
            return false;
        }
    }
}
