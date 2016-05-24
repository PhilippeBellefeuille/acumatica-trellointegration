using System;
using PX.Data;
using PX.SM;

namespace PX.TrelloIntegration
{
	[Serializable]
	public class TrelloListMapping : PX.Data.IBqlTable
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

        #region ListID
        public abstract class listID : PX.Data.IBqlField
        {
        }

        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(TrelloBoardMapping.listCntr))]
        public virtual int? ListID { get; set; }
        #endregion

        #region TrelloListID
        public abstract class trelloListID : PX.Data.IBqlField
		{
		}

		[PXDBString(30, IsUnicode = true)]
		[PXUIField(DisplayName = "Trello List")]
        [PXDefault]
        [PXTrelloListSelector(typeof(TrelloListMapping.boardID))]
		public virtual string TrelloListID { get; set; }
        #endregion

        #region ScreenID
        public abstract class screenID : IBqlField
        {
        }

        [PXDBString(8, IsFixed = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Screen ID", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        public virtual String ScreenID { get; set; }
        #endregion
        #region StepID
        public abstract class stepID : IBqlField
        {
        }

        [PXDBString(64, IsUnicode = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Step ID", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        [PXSelector(typeof(Search<AUStep.stepID, Where<AUStep.screenID, Equal<Optional<TrelloListMapping.screenID>>>>))]
        public virtual String StepID { get; set; }
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
