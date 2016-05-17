using System;
using PX.Data;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
	[Serializable]
	public class TrelloBoardMapping : PX.Data.IBqlTable
	{
		#region BoardID
		public abstract class boardID : PX.Data.IBqlField
		{
		}

		[PXDBIdentity(IsKey = true)]
		[PXUIField(Enabled = false)]
		public virtual int? BoardID { get; set; }
		#endregion
		#region CaseClassID
		public abstract class caseClassID : PX.Data.IBqlField
		{
		}

		[PXDBString(10, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Case Class ID")]
        [PXSelector(typeof(CRCaseClass.caseClassID),
                    DescriptionField = typeof(CRCaseClass.description))]
        public virtual string CaseClassID { get; set; }
        #endregion
        #region TrelloBoardID
        public abstract class trelloBoardID : PX.Data.IBqlField
		{
		}

		[PXDBString(30, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Trello Board")]
        [PXTrelloBoardSelector]
        [PXCheckUnique]
		public virtual string TrelloBoardID { get; set; }
		#endregion

    }
}
