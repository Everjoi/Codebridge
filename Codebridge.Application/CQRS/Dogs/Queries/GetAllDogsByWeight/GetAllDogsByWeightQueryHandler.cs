using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codebridge.Application.CQRS.Dogs.Queries.GetAllDogs;
using Codebridge.Application.DTOs;
using Codebridge.Application.Interfaces.Repository;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetAllDogsByWeight
{
    public class GetAllDogsByWeightQueryHandler:IRequestHandler<GetAllDogsByWeightQuery,Result<List<DogDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDogsByWeightQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task<Result<List<DogDto>>> Handle(GetAllDogsByWeightQuery request,CancellationToken cancellationToken)
        {
            
            var query = _unitOfWork.Repository<Dog>().Entities;

            if(request.Attribute == "weight")
            {
                if(request.Order == "desc")
                {
                    query = query.OrderByDescending(d => d.Weight);
                }
            }
            else
            {
                query = query.OrderBy(d => d.Weight);
            }


            var dogs = await query
                .ProjectTo<DogDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


            return await Result<List<DogDto>>.SuccessAsync(dogs);

        }
    }
}
