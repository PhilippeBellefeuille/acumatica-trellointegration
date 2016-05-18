﻿namespace PX.TrelloIntegration
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using PX.SM;
	using PX.Data;
	
	[System.SerializableAttribute()]
	public class TrelloSetup : PX.Data.IBqlTable
	{
        #region TrelloUsrToken
        public abstract class trelloUsrToken : PX.Data.IBqlField
		{
		}

		[PXDBString(64, IsUnicode = true)]
		[PXUIField(DisplayName = "Usr Token")]
		public virtual string TrelloUsrToken { get; set; }
        #endregion
        #region TrelloOrganizationID
        public abstract class trelloOrganizationID : PX.Data.IBqlField
		{
		}

		[PXDBString(30, IsUnicode = true)]
		[PXUIField(DisplayName = "Organization")]
        [PXTrelloOrganizationSelector(UseSetupCache = true)]
        public virtual string TrelloOrganizationID { get; set; }
        #endregion

        #region ConnectionDateTime
        public abstract class connectionDateTime : PX.Data.IBqlField
        {
        }

        [PXDBDateAndTime]
        [PXUIField(DisplayName = "Connected on")]
        public virtual DateTime? ConnectionDateTime { get; set; }
        #endregion

        #region UserName
        public abstract class userName : PX.Data.IBqlField
        {
        }

        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "User Name")]
        public virtual string UserName { get; set; }
        #endregion

        #region System Fields

        #region tstamp
        public abstract class Tstamp : PX.Data.IBqlField
        {
        }

        [PXDBTimestamp()]
        public virtual Byte[] tstamp { get; set; }
        #endregion
        #region CreatedByID
        public abstract class createdByID : PX.Data.IBqlField
        {
        }

        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        #endregion
        #region CreatedByScreenID
        public abstract class createdByScreenID : PX.Data.IBqlField
        {
        }

        [PXDBCreatedByScreenID()]
        public virtual String CreatedByScreenID { get; set; }
        #endregion
        #region CreatedDateTime
        public abstract class createdDateTime : PX.Data.IBqlField
        {
        }

        [PXDBCreatedDateTime]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion
        #region LastModifiedByID
        public abstract class lastModifiedByID : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion
        #region LastModifiedByScreenID
        public abstract class lastModifiedByScreenID : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedByScreenID()]
        public virtual String LastModifiedByScreenID { get; set; }
        #endregion
        #region LastModifiedDateTime
        public abstract class lastModifiedDateTime : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedDateTime]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #endregion
    }
}
