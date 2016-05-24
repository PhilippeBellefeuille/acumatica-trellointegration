using PX.Data;
using System;
using System.Collections;

namespace PX.TrelloIntegration
{
    public class CRTrelloSelect<TPrimaryNoteID> : PXSelect<TrelloCardMapping, 
                                                    Where<TrelloCardMapping.refNoteID, 
                                                        Equal<Current<TPrimaryNoteID>>>>
        where TPrimaryNoteID : IBqlField
    {
        public Type PrimaryView { get; }

        #region DataViews

        public PXSelect<TrelloBoardMapping> Boards;
        public PXSelect<TrelloListMapping,
                    Where<TrelloListMapping.boardID,
                        Equal<Optional<TrelloBoardMapping.boardID>>>> Lists;

        #endregion

        #region CTOR

        public CRTrelloSelect(PXGraph graph) : base(graph)
        {
            PrimaryView = BqlCommand.GetItemType(typeof(TPrimaryNoteID));
            graph.Initialized += OnGraphInitialized;
        }

        private void OnGraphInitialized(PXGraph graph)
        {
            graph.RowSelected.AddHandler(PrimaryView, PrimaryView_RowSelected);
            graph.RowInserted.AddHandler(PrimaryView, PrimaryView_RowInserted);
            InitializeDataViews(graph);
        }

        private void InitializeDataViews(PXGraph graph)
        {
            Boards = new PXSelect<TrelloBoardMapping>(graph, new PXSelectDelegate(delegate () { return boards(graph); }));

            graph.Caches[typeof(TrelloBoardMapping)] = Boards.Cache;

            Lists = new PXSelect<TrelloListMapping, 
                            Where<TrelloListMapping.boardID, 
                                Equal<Optional<TrelloBoardMapping.boardID>>>>(graph);

            graph.Caches[typeof(TrelloListMapping)] = Lists.Cache;
        }

        #endregion

        #region Delegates

        protected static IEnumerable boards(PXGraph graph)
        {
            var boardType = BoardTypes.GetBoardTypeFromGraph(graph.GetType());
            if(boardType != null)
            {
                foreach (TrelloBoardMapping board in PXSelect<TrelloBoardMapping, 
                                                        Where<TrelloBoardMapping.boardType, 
                                                            Equal<Required<TrelloBoardMapping.boardType>>>>.Select(graph, boardType))
                    yield return board;
            }
        }

        #endregion

        #region Event Handlers

        private void PrimaryView_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            if(e.Row != null)
            {
                //CurrentSelected.NoteID = PXNoteAttribute.GetNoteID(sender, e.Row, null);
            }
        }

        private void PrimaryView_RowInserted(PXCache sender, PXRowInsertedEventArgs e)
        {
            foreach(TrelloBoardMapping board in Boards.Select())
            {
                var card = new TrelloCardMapping() { BoardID = board.BoardID };
                this.Insert(card); 
            }
        }

        #endregion
    }

}
