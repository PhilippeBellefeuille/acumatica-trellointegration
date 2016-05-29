using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.RestSharp;
using PX.Data;
using System.Collections.Generic;
using System.Linq;

namespace PX.TrelloIntegration.Trello
{
    public class TrelloRepository
    {
        public TrelloSetup Setup { get; internal set; }

        public TrelloRepository(object arg)
        {
            if(arg is TrelloSetup)
            {
                SetConnection((TrelloSetup)arg);
            }
            else if(arg is PXGraph)
            {
                SetConnection(PXSetup<TrelloSetup>.Select((PXGraph)arg));
            }
        }

        public void SetConnection(TrelloSetup setup)
        {
            if (setup != null && !string.IsNullOrEmpty(setup.TrelloUsrToken))
            {
                Setup = setup;
                Connect();
            }
        }

        public Member Member
        {
            get
            {
                return Setup != null ? Member.Me : null;
            }
        }

        public virtual BoardCollection BoardCollection
        {
            get
            {
                return Setup != null ? Member.Me.Boards : null;
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

    public abstract class TrelloRepository<TTrelloDac, TTrelloObject> : TrelloRepository
        where TTrelloDac : class, IBqlTable, ITrelloObject, new()
    {
        public TrelloRepository(object arg) : base(arg) { }

        public IEnumerable<TTrelloDac> GetAll(string parentID = "")
        {
            foreach (var obj in GetAllTrelloObject(parentID))
            {
                yield return To(obj);
            }
        }

        public TTrelloDac GetByID(string id)
        {
            return To(GetTrelloObjectByID(id));
        }

        public abstract IEnumerable<TTrelloObject> GetAllTrelloObject(string parentID = "");
        public abstract TTrelloObject GetTrelloObjectByID(string id);

        public abstract TTrelloDac To(TTrelloObject obj);
    }

    public abstract class TrelloOrganizationableRepository<TTrelloDac, TTrelloObject> : 
                            TrelloRepository<TTrelloDac, TTrelloObject>
        where TTrelloDac : class, IBqlTable, ITrelloObject, ITrelloOrganizationObject, new()
    {
        public TrelloOrganizationableRepository(object arg) : base(arg) { }

        private Organization _Organization;
        public Organization Organization
        {
            get
            {
                if (Setup == null || string.IsNullOrEmpty(Setup.TrelloOrganizationID))
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
                return base.BoardCollection;
            }
        }

        public override void Connect()
        {
            base.Connect();
            if (!string.IsNullOrEmpty(Setup.TrelloOrganizationID) && !Member.Me.Organizations.Any(org => org.Id == Setup.TrelloOrganizationID))
                throw new PXException(Messages.UserNotInOrganization);
        }
    }

    public abstract class TrelloUpdatableRepository<TTrelloDac, TTrelloObject> :
                            TrelloRepository<TTrelloDac, TTrelloObject>
        where TTrelloDac : class, IBqlTable, ITrelloObject, new()
    {
        public TrelloUpdatableRepository(object arg) : base(arg) { }
        
        public abstract string Insert(TTrelloDac obj);

        public abstract bool Update(TTrelloDac obj);

        public abstract bool Delete(TTrelloDac obj);
    }
}
