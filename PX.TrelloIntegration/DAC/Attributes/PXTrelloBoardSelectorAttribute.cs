using PX.Data;
using System;
using System.Collections.Generic;
using System.Collections;
using Manatee.Trello.ManateeJson;
using Manatee.Trello;
using Manatee.Trello.RestSharp;

namespace PX.TrelloIntegration
{
    public class PXTrelloBoardSelectorAttribute : PXCustomSelectorAttribute
    {
        public string UserToken { get; set; }
        [Serializable]
        public class TrelloBoard : IBqlTable
        {
            #region BoardID
            public abstract class boardID : PX.Data.IBqlField
            {
            }

            [PXString(15, IsUnicode = true)]
            [PXDefault]
            [PXUIField(DisplayName = "Trello Board", Visibility = PXUIVisibility.SelectorVisible)]
            public virtual string BoardID { get; set; }
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

        public PXTrelloBoardSelectorAttribute() : base(typeof(TrelloBoard.boardID)) { }

        public override void CacheAttached(PXCache sender)
        {
            var trelloSetup = (TrelloSetup)PXSetup<TrelloSetup>.Select(sender.Graph);
            UserToken = trelloSetup?.TrelloUsrToken;
            base.CacheAttached(sender);
        }

        public IEnumerable GetRecords()
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new RestSharpClientProvider();
            TrelloAuthorization.Default.AppKey = Properties.Settings.Default.TrelloAPIKey;
            TrelloAuthorization.Default.UserToken = this.UserToken;
            
            foreach(var board in Member.Me.Boards)
            {
                yield return new TrelloBoard
                {
                    BoardID = board.Id,
                    Name = board.Name,
                    Descr = board.Description
                };
            }
            
        }
    }
}
