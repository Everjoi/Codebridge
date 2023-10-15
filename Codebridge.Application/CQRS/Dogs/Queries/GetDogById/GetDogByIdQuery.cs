using Codebridge.Application.DTOs;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetDogById
{
    public record GetDogByIdQuery : IRequest<Result<DogDto>>
    {
        public Guid Id { get; set; }

        public GetDogByIdQuery()
        {

        }

        public GetDogByIdQuery(Guid id)
        {
            Id = id;
        }
    }

}
