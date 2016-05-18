using PX.Data;
using PX.TrelloIntegration.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manatee.Trello;

namespace PX.TrelloIntegration.Trello
{
    [Serializable]
    public class TrelloMember : IBqlTable, ITrelloObject
    {
        #region ID
        public abstract class id : PX.Data.IBqlField
        {
        }

        [PXString(15, IsUnicode = true)]
        [PXDefault]
        [PXUIField(Visible = false, Enabled = false)]
        public virtual string Id { get; set; }
        #endregion
        #region Name
        public abstract class name : PX.Data.IBqlField
        {
        }

        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Name", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Name { get; set; }
        #endregion
        #region Descr
        public abstract class descr : PX.Data.IBqlField
        {
        }

        [PXString(300, IsUnicode = true)]
        [PXUIField(DisplayName = "Descr", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string Descr { get; set; }

        #endregion
    }
}
