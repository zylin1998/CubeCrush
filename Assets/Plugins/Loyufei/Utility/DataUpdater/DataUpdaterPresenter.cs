using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace Loyufei
{
    public class DataUpdaterPresenter 
    {
        public DataUpdaterPresenter(DataUpdater updater, DomainEventService service) 
        {
            Updater = updater;

            service.Register<UpdateData>(Update, GroupId);
        }

        public virtual object GroupId { get; }

        public DataUpdater Updater { get; }

        public void Update(UpdateData update) 
        {
            Updater.Update(update.Id, update.Value);
        }
    }

    public class UpdateData : DomainEventBase 
    {
        public UpdateData(object id, object value) 
        {
            Id    = id;
            Value = value;
        }

        public object Id    { get; }
        public object Value { get; }
    }
}