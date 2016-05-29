using PX.Data;
using System;

namespace PX.TrelloIntegration.Trello
{
    [Serializable]
    public class TrelloCard : IBqlTable, ITrelloObject
    {
        #region ID
        public abstract class id : PX.Data.IBqlField
        {
        }

        [PXString(15, IsUnicode = true, IsKey = true)]
        [PXDefault]
        [PXUIField(Visible = false, Enabled = false)]
        public virtual string Id { get; set; }
        #endregion

        #region ListID
        public abstract class listID : PX.Data.IBqlField
        {
        }

        [PXString(15, IsUnicode = true)]
        [PXDefault]
        [PXUIField(Visible = false, Enabled = false)]
        public virtual string ListId { get; set; }
        #endregion

        #region Name
        public abstract class name : PX.Data.IBqlField
        {
        }

        [PXString]
        [PXUIField(DisplayName = "Name", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Name { get; set; }
        #endregion

        #region Body
        public abstract class description : PX.Data.IBqlField
        {
        }

        [PXString]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        #endregion
    }
}
