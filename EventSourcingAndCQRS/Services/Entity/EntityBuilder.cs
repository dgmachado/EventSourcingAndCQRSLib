using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Models.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Entity
{
    public abstract class EntityBuilder<TEntity> : IEntityBuilder<TEntity>
        where TEntity : IEntity, new()
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected abstract void Apply(TEntity entity, ISnapshot<TEntity> snapshot);

        public TEntity BuildFromHistory(IEnumerable<IEvent> events)
        {
            var result = new TEntity();
            var entityType = typeof(TEntity).AssemblyQualifiedName;
            Log.Debug("Starting building entity '{0}' from historical events!", entityType);
            if (events.Any())
            {
                foreach (var evnt in events.OrderBy(s => s.Version))
                {
                    ApplyChange(result, evnt);
                }
            }
            Log.Debug("Finished build entity '{0}' from historical events!", entityType);
            return result;
        }

        private void RewriteEntityVersion(TEntity entity, int entityVersion)
        {
            entity.Version = entityVersion;
        }

        protected void ApplyChange(TEntity entity, IEvent evnt)
        {
            if (CanApplyEvent(entity, evnt))
            {
                ApplyEventToEntity(entity, evnt);
                RewriteEntityVersion(entity, evnt.Version);
            }
            else
            {
                ThrowApplyingEventWithUnexpectedVersionException(entity, evnt);
            }
        }

        private bool CanApplyEvent(TEntity entity, IEvent evnt)
        {
            if (evnt.IsSnapshot())
            {
                return IsUpdatedEvent(entity, evnt);
            }
            else
            {
                return EqualsNextExpectedVersion(entity, evnt.Version);
            }
        }

        private bool EqualsNextExpectedVersion(TEntity entity, int version)
        {
            return version == NextExpectedVersion(entity);
        }

        private static bool IsUpdatedEvent(TEntity entity, IEvent evnt)
        {
            return evnt.Version > entity.Version;
        }

        private int NextExpectedVersion(TEntity entity)
        {
            return entity.Version + 1;
        }

        private void ThrowApplyingEventWithUnexpectedVersionException(TEntity entity, IEvent evnt)
        {
            throw new ApplyingEventWithUnexpectedVersionException(entity.EntityId, entity.Version, evnt.Version);
        }

        private void ApplyEventToEntity(TEntity entity, IEvent evnt)
        {
            try
            {
                this.AsDynamic().Apply(entity, evnt);
            }
            catch (MissingMethodException)
            {
                throw new MissingMethodException(String.Format("The class '{0}' doesn't implement the method 'Apply' for the domain event '{1}'", this.GetType().FullName, evnt.GetType().FullName));
            }
        }
    }
}
