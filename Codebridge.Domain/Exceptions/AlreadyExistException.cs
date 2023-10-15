using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Domain.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(Type type, Guid id):base($"Item: {type} with id: {id} already exist")
        {
                
        }
    }
}
