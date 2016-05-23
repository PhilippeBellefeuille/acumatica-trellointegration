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
		public virtual int? BoardID { get; set; }
        #endregion
        #region BoardType
        public abstract class boardType : PX.Data.IBqlField
        {
        }

        [PXDBInt]
        [PXIntList(typeof(BoardTypes))]
        public virtual int? BoardType { get; set; }
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
        #region ListCntr
        public abstract class listCntr : PX.Data.IBqlField
        {
        }

        [PXDBInt()]
        [PXDefault(0)]
        public virtual Int32? ListCntr { get; set; }
        #endregion
        #region Description
        public abstract class description : PX.Data.IBqlField
        {
        }

        [PXDBString(50, InputMask = "", IsUnicode = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        [PXFormula(typeof(Default<TrelloBoardMapping.caseClassID>))]
        public virtual String Description { get; set; }
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
