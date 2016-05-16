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
        [PXTrelloOrganizationSelector]
        public virtual string TrelloOrganizationID { get; set; }
		#endregion
	}
}
