using System;
using PX.Data;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
	[Serializable]
	public class TrelloBoard : PX.Data.IBqlTable
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
		#region Name
		public abstract class name : PX.Data.IBqlField
		{
		}

		[PXDBString(200, IsUnicode = true)]
		[PXUIField(DisplayName = "Name")]
		public virtual string Name { get; set; }
        #endregion
        #region Descr
        public abstract class descr : PX.Data.IBqlField
		{
		}

		[PXDBString(IsUnicode = true)]
		[PXUIField(DisplayName = "Description")]
		public virtual string Descr { get; set; }
        #endregion
        #region UrlDescr
        public abstract class urlDescr : PX.Data.IBqlField
		{
		}

		[PXDBString(IsUnicode = true)]
		[PXUIField(DisplayName = "Url")]
		public virtual string UrlDescr { get; set; }
        #endregion
        #region Active
        public abstract class active : PX.Data.IBqlField
		{
		}
        
		[PXDBBool()]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Active")]
		public virtual bool? Active { get; set; }
        #endregion
    }
}
