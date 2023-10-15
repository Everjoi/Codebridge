using Codebridge.Application.DTOs;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetAllDogsByWeight
{
    public record GetAllDogsByWeightQuery:IRequest<Result<List<DogDto>>>
    {
        public string Attribute { get; set; } = "weight";
        public string Order { get; set; } = string.Empty;
    }
}
