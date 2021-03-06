﻿using PX.Data;
using System;

namespace PX.TrelloIntegration.Trello
{
    [Serializable]
    public class TrelloBoard : IBqlTable, ITrelloObject, ITrelloOrganizationObject
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
        #region OrganizationId
        public abstract class organizationId : PX.Data.IBqlField
        {
        }

        //Organization
        [PXString(15, IsUnicode = true)]
        [PXDefault]
        [PXUIField(Visible = false, Enabled = false)]
        public virtual string OrganizationId { get; set; }
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
