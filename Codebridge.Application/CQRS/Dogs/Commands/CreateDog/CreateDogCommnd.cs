using Codebridge.Application.Mappings;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Commands.CreateDog
{
    public record CreateDogCommnd : IRequest<Result<Guid>> , IMapFrom<Dog>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Colors { get; set; } = string.Empty;
        [Range(0,int.MaxValue,ErrorMessage = "Tail length must be a positive number.")]
        public double TailLength { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = "Weight must be greater or equal 1.")]
        public double Weight { get; set; }
    }
}
