using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using Manatee.Trello;
using PX.Data;
using PX.Common;
using System.Web.Script;
using System.Web.Script.Serialization;
using PX.TrelloIntegration.Trello;
using System.Web;

namespace PX.TrelloIntegration
{
    public class TrelloSetupMaint : PXGraph<TrelloSetupMaint>
    {
        public PXSave<TrelloSetup> Save;
        public PXCancel<TrelloSetup> Cancel;
        public PXSelect<TrelloSetup> Document;

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
            var setup = Document.Current;

            setup.TrelloUsrToken = token.Token;

            var memberRepo = new TrelloMemberRepository(setup);
            var currentMember = memberRepo.GetCurrentMember();

            setup.ConnectionDateTime = DateTime.Now;
            setup.UserName = currentMember.Name;

            Document.Update(setup);
            this.Actions.PressSave();

            return adapter.Get();
        }


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
    }
}