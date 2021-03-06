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
        public PXSelect<TrelloBoardMapping> Boards;
        public PXSelect<TrelloBoardMapping, 
                    Where<TrelloBoardMapping.boardID, 
                        Equal<Current<TrelloBoardMapping.boardID>>>> CurrentBoard;
        public PXSelect<TrelloBoardMapping,
                    Where<TrelloBoardMapping.parentBoardID,
                        Equal<Optional<TrelloBoardMapping.boardID>>>> ChildBoards;
        public PXSelect<TrelloListMapping, 
                    Where<TrelloListMapping.boardID, 
                        Equal<Optional<TrelloBoardMapping.boardID>>>> List;

        #endregion

        #region Delegates

        protected virtual IEnumerable boards(
            [PXInt]
            int? boardID
        )
        {
            if (boardID == null)
            {
                yield return new TrelloBoardMapping()
                {
                    ParentBoardID = 0,
                    BoardID = 0
                };
            }
            else
            {
                foreach (TrelloBoardMapping trelloBoard in ChildBoards.Select(boardID))
                {
                    yield return trelloBoard;
                }
            }

        }
        protected virtual IEnumerable currentBoard()
        {
            if (Boards.Current != null)
            {
                var isNotRoot = Boards.Current.BoardID != 0;
                var isMainTypeMapping = Boards.Current.ParentBoardID == 0;
                var hasChildBoard = ChildBoards.Select(Boards.Current.BoardID).Any();
                var isCompleted = isMainTypeMapping
                                  ? Boards.Current.BoardType.HasValue
                                  : !string.IsNullOrEmpty(Boards.Current.ClassID) &&
                                    !string.IsNullOrEmpty(Boards.Current.TrelloBoardID);
                var typeOrClassFilled = isMainTypeMapping 
                                        ? Boards.Current.BoardType.HasValue
                                        : !string.IsNullOrEmpty(Boards.Current.ClassID);
                var trelloBoardIDFilled = !string.IsNullOrEmpty(Boards.Current.TrelloBoardID);


                AddBoard.SetEnabled(!isNotRoot || (isMainTypeMapping && isCompleted));
                DeleteBoard.SetEnabled(isNotRoot);

                PopulateStates.SetEnabled(trelloBoardIDFilled);
                
                PXUIFieldAttribute.SetVisible<TrelloBoardMapping.boardType>(Caches[typeof(TrelloBoardMapping)], null, isMainTypeMapping);
                PXUIFieldAttribute.SetVisible<TrelloBoardMapping.classID>(Caches[typeof(TrelloBoardMapping)], null, !isMainTypeMapping);

                PXUIFieldAttribute.SetEnabled<TrelloBoardMapping.boardType>(Caches[typeof(TrelloBoardMapping)], null, isNotRoot && !hasChildBoard && !trelloBoardIDFilled);
                PXUIFieldAttribute.SetEnabled<TrelloBoardMapping.classID>(Caches[typeof(TrelloBoardMapping)], null, isNotRoot);
                PXUIFieldAttribute.SetEnabled<TrelloBoardMapping.trelloBoardID>(Caches[typeof(TrelloBoardMapping)], null, isNotRoot && typeOrClassFilled);

                Caches[typeof(TrelloBoardMapping)].AllowInsert = isNotRoot;
                Caches[typeof(TrelloBoardMapping)].AllowDelete = isNotRoot;
                Caches[typeof(TrelloBoardMapping)].AllowUpdate = isNotRoot;

                foreach (TrelloBoardMapping item in PXSelect<TrelloBoardMapping,
                                                        Where<TrelloBoardMapping.boardID, 
                                                            Equal<Required<TrelloBoardMapping.boardID>>>>.
                                                    Select(this, Boards.Current.BoardID))
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
                
                CurrentBoard.AllowUpdate = CurrentBoard.AllowUpdate && isConnected;

                List.AllowUpdate = isConnected;

                AddBoard.SetEnabled(AddBoard.GetEnabled() && isConnected);
                DeleteBoard.SetEnabled(DeleteBoard.GetEnabled() && isConnected);
                PopulateStates.SetEnabled(PopulateStates.GetEnabled() && isConnected);
            }
        }

        public virtual void TrelloBoardMapping_TrelloBoardID_FieldUpdated(PXCache sender, PXFieldUpdatedEventArgs e)
        {
            UpdateStates((TrelloBoardMapping)e.Row, true);
        }

        public virtual void TrelloBoardMapping_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            var row = (TrelloBoardMapping)e.Row;

            PXDefaultAttribute.SetPersistingCheck<TrelloBoardMapping.classID>(
                        sender,
                        row,
                        row.ParentBoardID == 0 ?
                            PXPersistingCheck.Nothing :
                            PXPersistingCheck.NullOrBlank);

            PXDefaultAttribute.SetPersistingCheck<TrelloBoardMapping.trelloBoardID>(
                        sender,
                        row,
                        row.ParentBoardID == 0 ?
                            PXPersistingCheck.Nothing :
                            PXPersistingCheck.NullOrBlank);
        }

        public override void Persist()
        {
            base.Persist();
            foreach (TrelloBoardMapping item in Caches[typeof(TrelloBoardMapping)].Cached)
            {
                if (item.TempParentID < 0)
                {
                    foreach (TrelloBoardMapping item2 in Caches[typeof(TrelloBoardMapping)].Cached)
                    {
                        if (item2.TempChildID == item.TempParentID)
                        {
                            item.ParentBoardID = item2.BoardID;
                            item.TempParentID = item2.BoardID;
                            Caches[typeof(TrelloBoardMapping)].SetStatus(item, PXEntryStatus.Updated);
                        }
                    }
                }
            }
            base.Persist();
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
            var selectedBoard = Boards.Current;
            if (selectedBoard.ParentBoardID == 0)
            {
                var inserted = (TrelloBoardMapping)Caches[typeof(TrelloBoardMapping)].Insert(new TrelloBoardMapping
                {
                    BoardType = Boards.Current.BoardType,
                    ParentBoardID = Boards.Current.BoardID
                });

                inserted.TempChildID = inserted.BoardID;
                inserted.TempParentID = inserted.ParentBoardID;

                Boards.Cache.ActiveRow = inserted;
            }
            return adapter.Get();
        }

        public PXAction<TrelloSetup> DeleteBoard;
        [PXUIField(DisplayName = " ", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Enabled = true)]
        [PXButton()]
        public virtual IEnumerable deleteBoard(PXAdapter adapter)
        {
            var selectedBoard = Boards.Current;
            if(selectedBoard.BoardID != 0)
            {
                if(selectedBoard.ParentBoardID == 0)
                {
                    var childrenBoards = ChildBoards
                                         .Select(selectedBoard.BoardID)
                                         .Select(br => (TrelloBoardMapping)br).ToList();

                    if (childrenBoards.Any())
                    {
                        if (Document.Ask(Messages.ValidationDeleteChildren, MessageButtons.YesNo) == WebDialogResult.Yes)
                        {
                            foreach(var childrenBoard in childrenBoards)
                            {
                                Caches[typeof(TrelloBoardMapping)].Delete(childrenBoard);
                            }
                            Caches[typeof(TrelloBoardMapping)].Delete(selectedBoard);
                        }
                    }
                    else
                    {
                        Caches[typeof(TrelloBoardMapping)].Delete(selectedBoard);
                    }
                }
                else
                {
                    Caches[typeof(TrelloBoardMapping)].Delete(selectedBoard);
                }
            }

            return adapter.Get();
        }

        public PXAction<TrelloSetup> PopulateStates;
        [PXButton]
        [PXUIField(DisplayName = "Reload")]
        public virtual void populateStates()
        {
            var board = (TrelloBoardMapping)Caches[typeof(TrelloBoardMapping)].Current;
            UpdateStates(board, false);
        }

        #endregion

        #region Configuration Helpers

        public virtual void UpdateStates(TrelloBoardMapping board, bool purgeList)
        {
            if(board != null)
            {
                if (purgeList || string.IsNullOrEmpty(board.TrelloBoardID))
                {
                    foreach (TrelloListMapping list in List.Select(board.BoardID))
                        List.Delete(list);
                }

                if(!string.IsNullOrEmpty(board.TrelloBoardID))
                {
                    var listMapping = List.Select(board.BoardID)
                              .Select(tl => (TrelloListMapping)tl)
                              .ToList();

                    var currentSteps = listMapping
                                          .Select(tl => Tuple.Create(tl.ScreenID, tl.StepID))
                                          .ToList();

                    var systemSteps = PXSelect<AUStep,
                                            Where<AUStep.screenID,
                                                Equal<Required<AUStep.screenID>>>>
                                          .Select(this, BoardTypes.GetBoardTypeScreenID(board.BoardType.GetValueOrDefault()))
                                          .Select(aus => Tuple.Create(((AUStep)aus).ScreenID, ((AUStep)aus).StepID))
                                          .ToList();

                    //Remove deprecated mapping
                    foreach (var list in listMapping.Where(tl => !systemSteps.Contains(Tuple.Create(tl.ScreenID, tl.StepID))))
                        List.Delete(list);

                    //Add new Mapping
                    foreach (var step in systemSteps.Where(st => !currentSteps.Contains(st)))
                        List.Insert(new TrelloListMapping() { BoardID = board.BoardID, ScreenID = step.Item1, StepID = step.Item2 });
                }
            }
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
    }
}