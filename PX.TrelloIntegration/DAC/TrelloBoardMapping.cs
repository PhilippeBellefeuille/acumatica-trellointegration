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
        #region ParentBoardID
        public abstract class parentBoardID : PX.Data.IBqlField
        {
        }

        [PXDBInt()]
        [PXDefault(0)]
        public virtual int? ParentBoardID { get; set; }
        #endregion
        #region BoardType
        public abstract class boardType : PX.Data.IBqlField
        {
        }

        [PXDBInt]
        [PXTrelloBoardType]
        [PXDefault]
        [PXUIField(DisplayName = "Board Type")]
        public virtual int? BoardType { get; set; }
        #endregion
        #region ClassID
        public abstract class classID : PX.Data.IBqlField
		{
		}


        [PXDBString(10, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Class ID")]
        [PXTrelloBoardClassSelector]
        public virtual string ClassID { get; set; }
        #endregion
        #region TrelloBoardID
        public abstract class trelloBoardID : PX.Data.IBqlField
		{
		}

		[PXDBString(30, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Trello Board")]
        [PXTrelloBoardSelector]
        [PXCheckUnique(typeof(classID), typeof(boardType), IgnoreNulls = false)]
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
        #region UserCntr
        public abstract class userCntr : PX.Data.IBqlField
        {
        }

        [PXDBInt()]
        [PXDefault(0)]
        public virtual Int32? UserCntr { get; set; }
        #endregion
        #region DisplayName
        public abstract class displayName : PX.Data.IBqlField
        {
        }

        [PXTrelloBoardDisplayName]
        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        public virtual String DisplayName { get; set; }
        #endregion

        #region temp

        #region TempChildID
        public abstract class tempChildID : PX.Data.IBqlField
        {
        }
        protected int? _TempChildID;
        [PXInt]
        public virtual int? TempChildID
        {
            get
            {
                return this._TempChildID;
            }
            set
            {
                this._TempChildID = value;
            }
        }
        #endregion

        #region TempparentID
        public abstract class tempParentID : PX.Data.IBqlField
        {
        }
        protected int? _TempParentID;
        [PXInt]
        public virtual int? TempParentID
        {
            get
            {
                return this._TempParentID;
            }
            set
            {
                this._TempParentID = value;
            }
        }
        #endregion

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
