using Codebridge.Domain.Common;
using Codebridge.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Domain.Entities
{
    public class Dog : BaseAuditableEntity 
    {
        public string Name { get; set; } = string.Empty;
        public string Colors { get; set; }
        public double TailLength { get; set; }
        public double Weight { get; set; }
    }
}
