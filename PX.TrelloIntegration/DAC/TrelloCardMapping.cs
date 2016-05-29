using System;
using PX.Data;
using PX.SM;
using PX.Objects.CR;

namespace PX.TrelloIntegration
{
	[Serializable]
	public class TrelloCardMapping : PX.Data.IBqlTable
	{
		#region BoardID
		public abstract class boardID : PX.Data.IBqlField
		{
		}

		[PXDBInt(IsKey = true)]
        [PXParent(typeof(Select<TrelloBoardMapping, 
                            Where<TrelloBoardMapping.boardID, 
                                Equal<TrelloListMapping.boardID>>>))]
        [PXDBDefault(typeof(TrelloBoardMapping.boardID))]
		public virtual int? BoardID { get; set; }
        #endregion

        #region RefNoteID
        public abstract class refNoteID : PX.Data.IBqlField
        {
        }

        [PXDBGuid(IsKey = true)]
        [PXParent(typeof(Select<Contact, 
                            Where<Contact.noteID, 
                                Equal<TrelloCardMapping.refNoteID>>>))]
        [PXParent(typeof(Select<CROpportunity,
                            Where<CROpportunity.noteID,
                                Equal<TrelloCardMapping.refNoteID>>>))]
        [PXParent(typeof(Select<CRCase,
                            Where<CRCase.noteID,
                                Equal<TrelloCardMapping.refNoteID>>>))]
        [PXDBDefault(typeof(Contact.noteID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXDBDefault(typeof(CROpportunity.noteID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXDBDefault(typeof(CRCase.noteID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual Guid? RefNoteID { get; set; }
        #endregion

        #region TrelloCardID
        public abstract class trelloCardID : PX.Data.IBqlField
		{
		}

		[PXDBString(30, IsUnicode = true)]
		[PXUIField(DisplayName = "Trello Card", Enabled = false, Visible = false)]
		public virtual string TrelloCardID { get; set; }
        #endregion

        #region CurrentListID
        public abstract class currentListID : PX.Data.IBqlField
        {
        }

        [PXDBInt()]
        public virtual int? CurrentListID { get; set; }
        #endregion

        #region NewListID
        public abstract class newListID : PX.Data.IBqlField
        {
        }

        [PXDBInt()]
        [PXDefault]
        public virtual int? NewListID { get; set; }
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
