using System;
using PX.Data;
using PX.Objects.CR;
using PX.TrelloIntegration.Trello;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PX.TrelloIntegration
{
    public static class BoardTypes
    {
        public const int Lead = 1;
        public const int Opportunity = 2;
        public const int Case = 3;

        public static readonly int[] List = 
        {
            BoardTypes.Lead,
            BoardTypes.Opportunity,
            BoardTypes.Case
        };

        public static class UI
        {
            public static readonly string Lead = GetBoardTypeTitle(BoardTypes.Lead);
            public static readonly string Opportunity = GetBoardTypeTitle(BoardTypes.Opportunity);
            public static readonly string Case = GetBoardTypeTitle(BoardTypes.Case);

            public static readonly string[] List = 
            {
                BoardTypes.UI.Lead,
                BoardTypes.UI.Opportunity,
                BoardTypes.UI.Case
            };

        }

        public static Type GetBoardTypeClassDAC(int boardType)
        {
            switch (boardType)
            {
                case BoardTypes.Lead:
                    return typeof(CRContactClass);
                case BoardTypes.Opportunity:
                    return typeof(CROpportunityClass);
                default:
                    return typeof(CRCaseClass);
            }
        }

        public static Type GetBoardTypeGraph(int boardType)
        {
            switch(boardType)
            {
                case BoardTypes.Lead:
                    return typeof(LeadMaint);
                case BoardTypes.Opportunity:
                    return typeof(OpportunityMaint);
                default:
                    return typeof(CRCaseMaint);
            }
        }

        public static int? GetBoardTypeFromGraph(Type graph)
        {
            if (graph == typeof(LeadMaint))
                return BoardTypes.Lead;
            else if (graph == typeof(OpportunityMaint))
                return BoardTypes.Opportunity;
            else if (graph == typeof(CRCaseMaint))
                return BoardTypes.Case;
            else
                return null;
        }

        public static string GetBoardTypeTitle(int boardType)
        {
            return GetBoardTypeTitle(GetBoardTypeGraph(boardType));
        }

        public static string GetBoardTypeTitle(Type graph)
        {
            return PXSiteMap.Provider.FindSiteMapNode(graph)?.Title;
        }

        public static string GetBoardTypeScreenID(int boardType)
        {
            return GetBoardTypeScreenID(GetBoardTypeGraph(boardType));
        }

        public static string GetBoardTypeScreenID(Type graph)
        {
            return PXSiteMap.Provider.FindSiteMapNode(graph)?.ScreenID;
        }
    }

    public class PXTrelloBoardClassSelectorAttribute : PXCustomSelectorAttribute
    {
        public class DummyClass : IBqlTable
        {
            #region CTOR

            public DummyClass() { }

            public DummyClass(object obj)
            {
                if(obj is CRContactClass)
                {
                    var contactClass = (CRContactClass)obj;
                    this.ClassID = contactClass.ClassID;
                    this.Description = contactClass.Description;
                    this.IsInternal = contactClass.IsInternal;
                }
                if (obj is CROpportunityClass)
                {
                    var opportunityClass = (CROpportunityClass)obj;
                    this.ClassID = opportunityClass.CROpportunityClassID;
                    this.Description = opportunityClass.Description;
                    this.IsInternal = opportunityClass.IsInternal;
                }
                if (obj is CRCaseClass)
                {
                    var caseClass = (CRCaseClass)obj;
                    this.ClassID = caseClass.CaseClassID;
                    this.Description = caseClass.Description;
                    this.IsInternal = caseClass.IsInternal;
                }
            }

            #endregion

            #region ClassID
            public abstract class classID : PX.Data.IBqlField
            {
            }

            [PXString(10, IsUnicode = true, IsKey = true)]
            [PXUIField(DisplayName = "Class ID", Visibility = PXUIVisibility.SelectorVisible)]
            public virtual String ClassID { get; set; }
            #endregion
            #region Description
            public abstract class description : PX.Data.IBqlField
            {
            }

            [PXString(255, IsUnicode = true)]
            [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
            public virtual String Description { get; set; }
            #endregion

            #region IsInternal
            public abstract class isInternal : PX.Data.IBqlField { }

            [PXBool]
            [PXUIField(DisplayName = "Internal", Visibility = PXUIVisibility.SelectorVisible)]
            public virtual Boolean? IsInternal { get; set; }
            #endregion
        }

        public PXTrelloBoardClassSelectorAttribute() : base(typeof(DummyClass.classID)) { }
        
        public IEnumerable GetRecords()
        {
            var current = (TrelloBoardMapping)_Graph.Caches[_BqlTable].Current;
            if(current != null && current.BoardType.HasValue)
            {
                var classTable = BoardTypes.GetBoardTypeClassDAC(current.BoardType.Value);

                var select = BqlCommand.Compose(
                    typeof(Select<>),
                        classTable
                );

                var cmd = BqlCommand.CreateInstance(select);
                PXView view = new PXView(_Graph, true, cmd);

                foreach (var row in view.SelectMulti())
                {
                    yield return new DummyClass(row);
                }
            }
        }
    }

    public class PXTrelloBoardTypeAttribute: PXIntListAttribute
    {
        public PXTrelloBoardTypeAttribute()
        {
            _AllowedValues = BoardTypes.List;
            _AllowedLabels = BoardTypes.UI.List;
        }
    }

    public class PXTrelloBoardDisplayNameAttribute : PXStringAttribute, 
                                                     IPXFieldSelectingSubscriber
    {
        public PXTrelloBoardDisplayNameAttribute() : base(100)
        {
        }

        public override void FieldSelecting(PXCache sender, PXFieldSelectingEventArgs e)
        {
            base.FieldSelecting(sender, e);

            var row = (TrelloBoardMapping)e.Row;
            if(row != null)
            {
                if (row.ParentBoardID == 0)
                {
                    if (row.BoardID == 0)
                    {
                        e.ReturnValue = PXSiteMap.RootNode.Title;
                    }
                    else if (row.BoardType == null)
                    {
                        e.ReturnValue = PXLocalizer.Localize(Messages.NewBoard);
                    }
                    else 
                    {
                        e.ReturnValue = GetNameWithTrelloBoardName(
                                            BoardTypes.GetBoardTypeTitle(row.BoardType.GetValueOrDefault()),
                                            sender,
                                            row);
                    }
                }
                else
                {
                    if(string.IsNullOrEmpty(row.ClassID))
                    {
                        e.ReturnValue = PXLocalizer.Localize(Messages.NewBoard);
                    }
                    else
                    {
                        e.ReturnValue = GetNameWithTrelloBoardName(
                                            row.ClassID,
                                            sender,
                                            row);
                    }
                }
            }
        }

        private static string GetNameWithTrelloBoardName(string name, PXCache sender, TrelloBoardMapping row)
        {
            if (string.IsNullOrEmpty(row.TrelloBoardID))
            {
                return name;
            }
            else
            {
                var trelloBoard = (TrelloBoard)PXSelectorAttribute.Select<TrelloBoardMapping.trelloBoardID>(sender, row);
                if(trelloBoard == null)
                {
                    return name;
                }
                else
                {
                    return string.Format("{0} - {1}", name, trelloBoard.Name);
                }
            }
        }
    }
}
