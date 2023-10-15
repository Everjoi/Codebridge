using Codebridge.Application.Mappings;
using Codebridge.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Codebridge.Application.DTOs
{
    public class DogDto : IMapFrom<Dog>
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = string.Empty;
        public string Colors { get; init; } = string.Empty;
        public double TailLength { get; init; }
        public double Weight { get; init; }
    }

}
