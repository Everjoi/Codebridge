using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codebridge.Application.DTOs;
using Codebridge.Application.Extensions;
using Codebridge.Application.Interfaces.Repository;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetDogsWithPagination
{
    public class GetDogsWithPaginationQueryHandler:IRequestHandler<GetDogsWithPaginationQuery,PaginatedResult<DogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDogsWithPaginationQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<DogDto>> Handle(GetDogsWithPaginationQuery query,CancellationToken cancellationToken)
        {
            if(query.Argument == "weight" && query.Order =="desc")
            {
                return await _unitOfWork.Repository<Dog>().Entities
                   .OrderByDescending(x => x.Weight)
                   .ProjectTo<DogDto>(_mapper.ConfigurationProvider)
                   .ToPaginatedListAsync(query.PageNumber,query.PageSize,cancellationToken);
            }
            else if(query.Argument == "name" && query.Order == "desc")
            {
                return await _unitOfWork.Repository<Dog>().Entities
                   .OrderByDescending(x => x.Name)
                   .ProjectTo<DogDto>(_mapper.ConfigurationProvider)
                   .ToPaginatedListAsync(query.PageNumber,query.PageSize,cancellationToken);
            }
            else
            {
                return await _unitOfWork.Repository<Dog>().Entities
                   .OrderBy(x => x.Name)
                   .ProjectTo<DogDto>(_mapper.ConfigurationProvider)
                   .ToPaginatedListAsync(query.PageNumber,query.PageSize,cancellationToken);
            }
            
        }

    }
}
