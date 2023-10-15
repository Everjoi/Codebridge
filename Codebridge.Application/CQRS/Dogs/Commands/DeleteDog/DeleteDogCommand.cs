using Codebridge.Application.Mappings;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Commands.DeleteDog
{
    public record DeleteDogCommand : IRequest<Result<Guid>> ,IMapFrom<Dog>
    {
        public Guid Id { get; set; }

        public DeleteDogCommand()
        {

        }

        public DeleteDogCommand(Guid id)
        {
            Id = id;
        }
    }
}
