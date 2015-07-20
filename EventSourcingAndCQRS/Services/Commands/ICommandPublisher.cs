using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Services.Commands
{
    public interface ICommandPublisher
    {
        void Publish<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
