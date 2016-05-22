using PX.Data;
using PX.SM;
using System.Linq;

namespace PX.TrelloIntegration
{
    public class TrelloBoardMappingMaint : PXGraph<TrelloBoardMappingMaint, TrelloBoardMapping>
    {
        #region DataViews

        public PXSelect<TrelloBoardMapping> Board;
        public PXSelect<TrelloListMapping, Where<TrelloListMapping.boardID, Equal<Current<TrelloBoardMapping.boardID>>>> List;

        #endregion

        #region CTOR
        public TrelloBoardMappingMaint()
        {
            List.AllowInsert =
            List.AllowDelete = false;
        }
        #endregion

        #region Actions

        public PXAction<TrelloBoardMapping> PopulateStates;

        [PXButton]
        [PXUIField(DisplayName = "Populate")]
        public virtual void populateStates()
        {
            var listMapping = List.Select()
                                  .Select(tl => (TrelloListMapping)tl)
                                  .ToList();

            var currentSteps = listMapping
                                  .Select(tl => tl.StepID)
                                  .ToList();

            var systemSteps = PXSelect<AUStep,
                                    Where<AUStep.screenID,
                                        Equal<caseScreenID>>>
                                  .Select(this)
                                  .Select(aus => ((AUStep)aus).StepID)
                                  .ToList();

            //Remove deprecated mapping
            foreach (var list in listMapping.Where(tl => !systemSteps.Contains(tl.StepID)))
                List.Delete(list);

            //Add new Mapping
            foreach (var step in systemSteps.Where(st => !currentSteps.Contains(st)))
                List.Insert(new TrelloListMapping() { StepID = step });
        }

        #endregion
    }
}