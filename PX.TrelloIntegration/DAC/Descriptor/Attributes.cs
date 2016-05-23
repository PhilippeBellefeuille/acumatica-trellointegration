using System;
using PX.Data;
using PX.Objects.CR;
using PX.TrelloIntegration.Trello;

namespace PX.TrelloIntegration
{
    public class caseScreenID : Constant<string>
    {
        public caseScreenID()
            : base("CR306000")
        {
        }
    }

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

        public static Type GetBoardTypeGraph(int boardType)
        {
            switch(boardType)
            {
                case 1:
                    return typeof(LeadMaint);
                case 2:
                    return typeof(OpportunityMaint);
                default:
                    return typeof(CRCaseMaint);
            }
        }

        public static string GetBoardTypeTitle(int boardType)
        {
            return GetBoardTypeTitle(GetBoardTypeGraph(boardType));
        }

        public static string GetBoardTypeTitle(Type graph)
        {
            return PXSiteMap.Provider.FindSiteMapNode(graph)?.Title;
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
                    if(string.IsNullOrEmpty(row.CaseClassID))
                    {
                        e.ReturnValue = PXLocalizer.Localize(Messages.NewBoard);
                    }
                    else
                    {
                        e.ReturnValue = GetNameWithTrelloBoardName(
                                            row.CaseClassID,
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
