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
    internal class EntityBuilderActivator
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static EntityBuilder<TEntity> InstantiateEntityBuilderFor<TEntity>()
                    where TEntity : IEntity, new()
        {
            Log.Debug("Instantiating entityBuilder for entity '{0}'", typeof(TEntity));
            var entityBuilderType = typeof(TEntity).GetAttributeValue((EntityBuilderAttribute entityBuilderAttribute) => entityBuilderAttribute.EntityBuilderType);
            if (entityBuilderType != null)
            {
                var entityBuilder = Activator.CreateInstance(entityBuilderType) as EntityBuilder<TEntity>;
                if (entityBuilder != null)
                {
                    return entityBuilder;
                }
                else
                {
                    var exception = new Exception(String.Format("Type '{0}' has attribute '{0}' with entityBuilder different of '{1}'", typeof(EntityBuilderAttribute).FullName, typeof(EntityBuilder<TEntity>).FullName));
                    Log.Error(exception);
                    throw exception;
                }
            }
            else
            {
                var exception = new Exception(String.Format("Type '{0}' hasn't the attribute '{1}'", typeof(EntityBuilderAttribute).FullName));
                Log.Error(exception);
                throw exception;
            }
        }
    }
}
