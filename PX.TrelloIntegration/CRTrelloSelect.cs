using PX.Data;
using System;
using System.Collections;
using System.Linq;
using PX.TrelloIntegration.Trello;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
    public class CRTrelloSelect<TPrimaryTable, TPrimaryNoteID, TPrimaryClassID> 
                                              : PXSelect<TrelloCardMapping, 
                                                    Where<TrelloCardMapping.refNoteID, 
                                                        Equal<Current<TPrimaryNoteID>>>>
        where TPrimaryTable : class, IBqlTable, new()
        where TPrimaryNoteID : IBqlField
        where TPrimaryClassID : IBqlField
    {

        #region DataViews

        public PXSelect<TrelloBoardMapping> Boards;

        #endregion

        #region CTOR

        public CRTrelloSelect(PXGraph graph) : base(graph)
        {
            graph.Initialized += OnGraphInitialized;
        }

        private void OnGraphInitialized(PXGraph graph)
        {
            graph.RowInserted.AddHandler<TPrimaryTable>(PrimaryView_RowInserted);
            graph.RowUpdated.AddHandler<TPrimaryTable>(PrimaryView_RowUpdated);
            graph.RowPersisting.AddHandler<TrelloCardMapping>(TrelloCardMapping_RowPersisting);
            InitializeDataViews(graph);
        }

        private void InitializeDataViews(PXGraph graph)
        {
            Boards = new PXSelect<TrelloBoardMapping>(graph, new PXSelectDelegate(delegate () { return boards(graph); }));
        }

        #endregion

        #region Delegates

        protected static IEnumerable boards(PXGraph graph)
        {
            var boardType = BoardTypes.GetBoardFromDAC<TPrimaryTable>();
            var classID = GetPrimaryCache(graph).GetValue<TPrimaryClassID>(GetPrimaryCurrent(graph))?.ToString();

            if(boardType != null && !string.IsNullOrEmpty(classID))
            {
                var boardList = PXSelect<TrelloBoardMapping,
                                    Where<TrelloBoardMapping.boardType,
                                        Equal<Required<TrelloBoardMapping.boardType>>,
                                    And<TrelloBoardMapping.trelloBoardID,
                                        IsNotNull,
                                    And2<
                                        Where<TrelloBoardMapping.classID,
                                            IsNull>,
                                        Or<TrelloBoardMapping.classID,
                                            Equal<Required<TrelloBoardMapping.classID>>>>>>>
                                    .Select(graph, boardType, classID)
                                    .Select(b => (TrelloBoardMapping)b)
                                    .ToList();

                var parentBoardList = boardList.Select(b => b.ParentBoardID).Distinct().ToList();

                foreach(var board in boardList)
                {
                    if (!parentBoardList.Contains(board.BoardID))
                        yield return board;
                }
                    
            }
        }

        #endregion

        #region Event Handlers

        private void PrimaryView_RowInserted(PXCache sender, PXRowInsertedEventArgs e)
        {
            ReloadCards();
        }
        
        private void PrimaryView_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
        {
            ReloadCards();
        }

        private void TrelloCardMapping_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            var row = (TrelloCardMapping)e.Row;
            if(row != null)
            {
                var repo = new TrelloCardRepository(sender.Graph);
                if ((e.Operation == PXDBOperation.Insert || e.Operation == PXDBOperation.Update))
                {
                    if (string.IsNullOrEmpty(row.TrelloCardID))
                    {
                        row.TrelloCardID = repo.Insert(GetTrelloCard(sender.Graph, row));
                    }
                    else
                    {
                        repo.Update(GetTrelloCard(sender.Graph, row));
                    }

                    row.CurrentListID = row.NewListID;
                }
                else if (e.Operation == PXDBOperation.Delete)
                {
                    repo.Delete(GetTrelloCard(sender.Graph, row));
                }
            }
        }

        #endregion

        #region State Helpers

        private void ReloadCards()
        {
            var boards = Boards.Select().Select(brd => (TrelloBoardMapping)brd).ToList();
            var cards = this.Select().Select(crd => (TrelloCardMapping)crd).ToList();

            foreach(var card in cards.Where(crd => !boards.Select(brd=> brd.BoardID).Contains(crd.BoardID)))
            {
                this.Delete(card);
            }

            foreach(var board in boards.Where(brd => !cards.Select(crd => crd.BoardID).Contains(brd.BoardID)))
            {
                this.Insert(new TrelloCardMapping() { BoardID = board.BoardID });
            }

            foreach(TrelloCardMapping card in this.Select())
            {
                card.NewListID = GetNewListID(card.BoardID);
                this.Update(card);
            }
        }

        private int? GetNewListID(int? boardID)
        {
            TrelloListMapping list = PXSelect<TrelloListMapping,
                                        Where<TrelloListMapping.boardID,
                                            Equal<Required<TrelloBoardMapping.boardID>>, 
                                        And<TrelloListMapping.stepID, 
                                            Equal<Required<TrelloListMapping.stepID>>>>>
                                    .Select(_Graph, boardID, _Graph.AutomationStep);
            if (list == null)
                return null;
            else
                return list.ListID;
        }

        #endregion

        #region Cast Helpers

        public static TrelloCard GetTrelloCard(PXGraph graph, TrelloCardMapping mapping)
        {
            string name;
            string description;

            TrelloListMapping newList = PXSelect<TrelloListMapping,
                                            Where<TrelloListMapping.listID,
                                                Equal<Required<TrelloListMapping.listID>>>>.Select(graph, mapping.NewListID);

            if(newList != null && BoardTypes.GetBoardNameDescription<TPrimaryTable>(GetPrimaryCurrent(graph), out name, out description))
            {
                return new TrelloCard()
                {
                    Id = mapping.TrelloCardID,
                    ListId = newList.TrelloListID,
                    Name = name,
                    Description = description
                };
            }
            return null;
        }

        public static PXCache<TPrimaryTable> GetPrimaryCache(PXGraph graph)
        {
            return graph.Caches<TPrimaryTable>();
        }

        public static TPrimaryTable GetPrimaryCurrent(PXGraph graph)
        {
            return (TPrimaryTable)GetPrimaryCache(graph).Current;
        }

        #endregion
    }

}
