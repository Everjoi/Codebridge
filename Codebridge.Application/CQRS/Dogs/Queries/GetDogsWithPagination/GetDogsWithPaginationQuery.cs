using Codebridge.Application.DTOs;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetDogsWithPagination
{
    public record  GetDogsWithPaginationQuery:IRequest<PaginatedResult<DogDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetDogsWithPaginationQuery() { }

        public GetDogsWithPaginationQuery(int pageNumber,int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


    }
}
