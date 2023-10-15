using Codebridge.Domain.Common;
using Codebridge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Commands.CreateDog
{
    public class DogCreatedEvent : BaseEvent
    {
        public Dog Dog { get; }

        public DogCreatedEvent(Dog dog)
        {
            Dog = dog;
        }
    }
}
