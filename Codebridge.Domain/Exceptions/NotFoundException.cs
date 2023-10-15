﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type type,Guid id) : base ($"Item {type}: {id} not found")
        {
                
        }

    }
}
