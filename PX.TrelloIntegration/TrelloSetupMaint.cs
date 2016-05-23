using System;
using System.Collections;
using PX.Data;
using PX.TrelloIntegration.Trello;
using System.Web;
using System.Linq;
using PX.SM;
using System.Collections.Generic;

namespace PX.TrelloIntegration
{
    public class TrelloSetupMaint : PXGraph<TrelloSetupMaint>
    {
        
        #region DataViews

        public PXSave<TrelloSetup> Save;
        public PXCancel<TrelloSetup> Cancel;

        public PXSelect<TrelloSetup> Document;
        public PXSelect<TrelloBoardMapping> Folders;
        public PXSelect<TrelloBoardMapping, 
                    Where<TrelloBoardMapping.boardID, 
                        Equal<Current<TrelloBoardMapping.boardID>>>> CurrentBoard;
        public PXSelect<TrelloListMapping, 
                    Where<TrelloListMapping.boardID, 
                        Equal<Current<TrelloBoardMapping.boardID>>>> List;

        #endregion

        #region Delegates

        protected virtual IEnumerable folders(
            [PXInt]
            int? boardType,
            [PXInt]
            int? boardID
        )
        {
            if (boardType == null)
            {
                yield return new TrelloBoardMapping()
                {
                    BoardID = 0,
                    BoardType = (int)BoardTypes.Case,
                    Description = "Case"
                };
                yield return new TrelloBoardMapping()
                {
                    BoardID = 0,
                    BoardType = (int)BoardTypes.Lead,
                    Description = "Lead"
                };
            }
            else if(boardID == 0)
            {
                foreach (TrelloBoardMapping trelloBoard in PXSelect<TrelloBoardMapping,
                                                Where<TrelloBoardMapping.boardType,
                                                    Equal<Required<TrelloBoardMapping.boardType>>>>
                                                    .Select(this, boardType))
                {
                    yield return trelloBoard;
                }
            }
            //else 
            //{
            //    foreach (TrelloBoardMapping trelloBoard in PXSelect<TrelloBoardMapping,
            //                                    Where<TrelloBoardMapping.boardType,
            //                                        Equal<Required<TrelloBoardMapping.boardType>>, 
            //                                    And<TrelloBoardMapping.boardID,
            //                                        Equal<Required<TrelloBoardMapping.boardID>>>>>
            //                                        .Select(this, boardType, boardID))
            //    {
            //        yield return trelloBoard;
            //    }
            //}
        }
        protected virtual IEnumerable currentBoard()
        {
            if (Folders.Current != null)
            {
                //CurrentSelected.BoardType = Folders.Current.BoardType;
                CurrentSelected.BoardID = Folders.Current.BoardID;
            }

            AddBoard.SetEnabled(/*CurrentSelected.BoardType != null && */CurrentSelected.BoardID == 0);
            DeleteBoard.SetEnabled(/*CurrentSelected.BoardType != null && */CurrentSelected.BoardID != 0);

            if (CurrentSelected.BoardID != null)
            {
                PXUIFieldAttribute.SetEnabled<TrelloBoardMapping.caseClassID>(Caches[typeof(TrelloBoardMapping)], null, CurrentSelected.BoardID != 0);
                PXUIFieldAttribute.SetEnabled<TrelloBoardMapping.trelloBoardID>(Caches[typeof(TrelloBoardMapping)], null, CurrentSelected.BoardID != 0);
                Caches[typeof(TrelloBoardMapping)].AllowInsert = CurrentSelected.BoardID != 0;
                Caches[typeof(TrelloBoardMapping)].AllowDelete = CurrentSelected.BoardID != 0;
                Caches[typeof(TrelloBoardMapping)].AllowUpdate = CurrentSelected.BoardID != 0;

                foreach (TrelloBoardMapping item in PXSelect<TrelloBoardMapping,
                Where<TrelloBoardMapping.boardID, Equal<Required<TrelloBoardMapping.boardID>>>>.
                Select(this, CurrentSelected.BoardID))
                {
                    yield return item;
                }
            }
        }

        #endregion

        #region CTOR

        public TrelloSetupMaint()
        {
            List.AllowInsert =
            List.AllowDelete = false;
        }

        #endregion

        #region Event Handlers

        public virtual void TrelloSetup_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            var row = (TrelloSetup)e.Row;
            if(row != null)
            {
                var isConnected = !string.IsNullOrEmpty(row.TrelloUsrToken);
                login.SetVisible(!isConnected);
                logout.SetVisible(isConnected);
                PXUIFieldAttribute.SetEnabled<TrelloSetup.trelloOrganizationID>(sender, row, isConnected);
            }
        }

        public virtual void TrelloBoardMapping_Description_FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            var row = (TrelloBoardMapping)e.Row;
            if(!string.IsNullOrEmpty(row.CaseClassID))
            {
                e.NewValue = row.CaseClassID;
            }
        }

        #endregion

        #region Actions

        public PXAction<TrelloSetup> login;
        [PXUIField(DisplayName = "Login to Trello")]
        [PXButton(ImageKey = "LinkWB")]
        public virtual void Login()
        {
            Actions.PressSave();
            throw new PXRedirectToUrlException(TrelloUrl, PXBaseRedirectException.WindowMode.Same, "Login To Trello");
        }
        
        public PXAction<TrelloSetup> logout;
        [PXUIField(DisplayName = "Logout")]
        [PXButton(ImageKey = "LinkWB")]
        public virtual void Logout()
        {
            var setup = Document.Current;

            setup.ConnectionDateTime = null;
            setup.UserName = null;
            setup.TrelloUsrToken = null;

            Document.Update(setup);
            Actions.PressSave();
        }

        public PXAction<TrelloSetup> completeAuthentication;
        [PXButton()]
        public virtual IEnumerable CompleteAuthentication(PXAdapter adapter)
        {
            var token = TrelloToken.GetToken(adapter.CommandArguments);

            //If the user deny the authentication token will be blank
            if(!string.IsNullOrEmpty(token.Token))
            {
                var setup = Document.Current;

                setup.TrelloUsrToken = token.Token;

                var memberRepo = new TrelloMemberRepository(setup);
                var currentMember = memberRepo.GetCurrentMember();

                setup.ConnectionDateTime = DateTime.Now;
                setup.UserName = currentMember.Name;

                Document.Update(setup);
                this.Actions.PressSave();
            }

            return adapter.Get();
        }

        public PXAction<TrelloSetup> AddBoard;
        [PXUIField(DisplayName = " ", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Enabled = true)]
        [PXButton()]
        public virtual IEnumerable addBoard(PXAdapter adapter)
        {
            int boardType = (int)Folders.Current.BoardType;
            var inserted = (TrelloBoardMapping)Caches[typeof(TrelloBoardMapping)].Insert(new TrelloBoardMapping
            {
                Description = "<NEW>",
                BoardType = boardType,
            });
            //inserted.TempChildID = inserted.CategoryID;
            //inserted.TempParentID = boardType;
            //Folders.Cache.ActiveRow = inserted;
            Folders.Cache.ActiveRow = inserted;
            return adapter.Get();
        }

        public PXAction<TrelloSetup> DeleteBoard;
        [PXUIField(DisplayName = " ", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Enabled = true)]
        [PXButton()]
        public virtual IEnumerable deleteBoard(PXAdapter adapter)
        {
            Caches[typeof(TrelloBoardMapping)].Delete(Folders.Current);
            return adapter.Get();
        }

        public PXAction<TrelloSetup> PopulateStates;
        [PXButton]
        [PXUIField(DisplayName = "Populate")]
        public virtual void populateStates()
        {
            var listMapping = List.Select()
                                  .Select(tl => (TrelloListMapping)tl)
                                  .ToList();

            var currentSteps = listMapping
                                  .Select(tl => tl.StepID)
                                  .ToList();

            var systemSteps = PXSelect<AUStep,
                                    Where<AUStep.screenID,
                                        Equal<caseScreenID>>>
                                  .Select(this)
                                  .Select(aus => ((AUStep)aus).StepID)
                                  .ToList();

            //Remove deprecated mapping
            foreach (var list in listMapping.Where(tl => !systemSteps.Contains(tl.StepID)))
                List.Delete(list);

            //Add new Mapping
            foreach (var step in systemSteps.Where(st => !currentSteps.Contains(st)))
                List.Insert(new TrelloListMapping() { StepID = step });
        }

        #endregion

        #region Connection Helpers

        public string TrelloUrl
        {
            get
            {
                return String.Format("https://trello.com/1/authorize?expiration=never&name=Acumatica&callback_method=fragment&key={0}&return_url={1}&scope=read,write,account&response_type=token",
                                     Properties.Settings.Default.TrelloAPIKey,
                                     System.Web.HttpContext.Current.Request.UrlReferrer.GetLeftPart(UriPartial.Path) + "Frames/TrelloAuthenticator.html");
            }
        }

        public class TrelloToken
        {
            public string Token { get; }

            private TrelloToken(string token)
            {
                Token = token;
            }

            public static TrelloToken GetToken(string arg)
            {
                return new TrelloToken(HttpUtility.ParseQueryString(arg).Get("token"));
            }
        }

        #endregion

        #region Tree Helper

        public class SelectedNode : IBqlTable
        {
            //#region BoardType
            //public abstract class boardType : PX.Data.IBqlField
            //{
            //}

            //[PXInt(IsKey = true)]
            //[PXIntList(typeof(BoardTypes))]
            //public virtual int? BoardType { get; set; }
            //#endregion

            #region BoardID
            public abstract class boardID : PX.Data.IBqlField
            {
            }

            [PXInt(IsKey = true)]
            public virtual int? BoardID { get; set; }
            #endregion
        }

        private SelectedNode CurrentSelected
        {
            get
            {
                PXCache cache = this.Caches[typeof(SelectedNode)];
                if (cache.Current == null)
                {
                    cache.Insert();
                    cache.IsDirty = false;
                }
                return (SelectedNode)cache.Current;
            }
        }

        #endregion
    }
}