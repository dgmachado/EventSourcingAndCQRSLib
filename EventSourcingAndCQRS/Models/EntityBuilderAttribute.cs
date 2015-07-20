using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services.Domain;
using EventSourcingAndCQRS.Services.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityBuilderAttribute : Attribute
    {
        public Type EntityBuilderType { get; private set; }

        public EntityBuilderAttribute(Type entityBuilder)
        {
            EntityBuilderType = entityBuilder;
        }
    }
}
