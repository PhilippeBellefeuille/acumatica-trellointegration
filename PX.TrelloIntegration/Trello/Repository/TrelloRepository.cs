using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.RestSharp;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.TrelloIntegration.Trello
{
    public abstract class TrelloRepository<TTrelloDac, TTrelloObject>
        where TTrelloDac : class, IBqlTable, ITrelloObject, new()
    {
        public TrelloSetup Setup { get; }

        public TrelloRepository(PXGraph graph)
        {
            Setup = (TrelloSetup)PXSetup<TrelloSetup>.Select(graph);
            Connect();
        }

        public IEnumerable<TTrelloDac> GetAll()
        {
            foreach (var obj in GetAllTrelloObject())
            {
                yield return To(obj);
            }
        }

        public TTrelloDac GetByID(string id)
        {
            return To(GetTrelloObjectByID(id));
        }

        public abstract IEnumerable<TTrelloObject> GetAllTrelloObject();
        public abstract TTrelloObject GetTrelloObjectByID(string id);

        public abstract TTrelloDac To(TTrelloObject obj);

        public Member Member
        {
            get
            {
                return Member.Me;
            }
        }

        public virtual BoardCollection BoardCollection
        {
            get
            {
                return Member.Me.Boards;
            }
        }

        public virtual void Connect()
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new RestSharpClientProvider();
            TrelloAuthorization.Default.AppKey = Properties.Settings.Default.TrelloAPIKey;
            TrelloAuthorization.Default.UserToken = Setup.TrelloUsrToken;
            
        }
    }

    public abstract class TrelloOrganizationableRepository<TTrelloDac, TTrelloObject> : 
                            TrelloRepository<TTrelloDac, TTrelloObject>
        where TTrelloDac : class, IBqlTable, ITrelloObject, ITrelloOrganizationObject, new()
    {
        public TrelloOrganizationableRepository(PXGraph graph) : base(graph) { }

        private Organization _Organization;
        public Organization Organization
        {
            get
            {
                if (string.IsNullOrEmpty(Setup.TrelloOrganizationID))
                    return null;
                if (_Organization != null)
                    return _Organization;
                return _Organization = new Organization(Setup.TrelloOrganizationID);
            }
        }

        public override BoardCollection BoardCollection
        {
            get
            {
                if (Organization != null)
                    return Organization.Boards;
                return Member.Me.Boards;
            }
        }

        public override void Connect()
        {
            base.Connect();
            if (!string.IsNullOrEmpty(Setup.TrelloOrganizationID) && !Member.Me.Organizations.Any(org => org.Id == Setup.TrelloOrganizationID))
                throw new PXException("TODO: User not member of selected organization");
        }
    }
}
