using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using Manatee.Trello;
using PX.Data;
using PX.Common;

namespace PX.TrelloIntegration
{
    public class TrelloSetupMaint : PXGraph<TrelloSetupMaint>
    {
        public PXSave<TrelloSetup> Save;
        public PXCancel<TrelloSetup> Cancel;
        public PXSelect<TrelloSetup> TrelloSetup;

        public PXAction<TrelloSetup> login;
        [PXUIField(DisplayName = "Login to Trello")]
        [PXButton(ImageKey = "LinkWB")]
        public virtual void Login()
        {
            Actions.PressSave();
            throw new PXRedirectToUrlException(TrelloUrl, PXBaseRedirectException.WindowMode.Same, "Login To Trello");
        }

        public PXAction<TrelloSetup> completeAuthentication;
        [PXButton(ImageKey = "LinkWB")]
        public virtual IEnumerable CompleteAuthentication(PXAdapter adapter)
        {
            var token = adapter.CommandArguments.Replace("token=", string.Empty);
            
            foreach(TrelloSetup row in adapter.Get())
            {
                row.TrelloUsrToken = token;
                TrelloSetup.Update(row);
                this.Actions.PressSave();

                yield return row;
            }
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
    }
}