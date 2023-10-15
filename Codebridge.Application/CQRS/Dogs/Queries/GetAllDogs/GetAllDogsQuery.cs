using Codebridge.Application.DTOs;
using Codebridge.Application.Mappings;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetAllDogs
{
    public record GetAllDogsQuery:IRequest<Result<List<DogDto>>>;
    
}
