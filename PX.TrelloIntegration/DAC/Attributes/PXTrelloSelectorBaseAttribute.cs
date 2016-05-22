using PX.Data;
using System;
using System.Collections;
using PX.TrelloIntegration.Trello;

namespace PX.TrelloIntegration
{
    public abstract class PXTrelloSelectorBaseAttribute : PXCustomSelectorAttribute, IPXRowSelectedSubscriber
    {
        public bool UseSetupCache { get; set; }
        public TrelloRepository Repository { get; internal set; }

        public PXTrelloSelectorBaseAttribute(Type type) : base(type) { }
        public PXTrelloSelectorBaseAttribute(Type type, Type[] fieldList) : base(type, fieldList) { }

        public abstract Type RepositoryType { get; }
        public abstract IEnumerable GetTrelloRecords();

        public override void CacheAttached(PXCache sender)
        {
            if(!UseSetupCache)
            {
                SetRepository(sender.Graph);
            }
            
            base.CacheAttached(sender);
        }

        public void RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            if (e.Row != null)
            {
                PXUIFieldAttribute.SetEnabled(sender, e.Row, _FieldName, Repository != null);

                if(UseSetupCache && Repository == null)
                {
                    var trelloSetup = (TrelloSetup)sender.Graph.Caches[typeof(TrelloSetup)].Current;
                    SetRepository(trelloSetup);
                }
            }
        }

        private void SetRepository(object arg)
        {
            var repository = (TrelloRepository)Activator.CreateInstance(RepositoryType, arg);
            
            if (repository != null && repository.Setup != null)
            {
                Repository = repository;
            }
        }

        public IEnumerable GetRecords()
        {
            if(Repository != null)
            {
                foreach(var trelloRecord in GetTrelloRecords())
                {
                    yield return trelloRecord;
                }
            }
        }
    }
}
