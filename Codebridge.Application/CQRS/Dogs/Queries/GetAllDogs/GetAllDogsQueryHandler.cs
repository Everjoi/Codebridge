using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codebridge.Application.DTOs;
using Codebridge.Application.Interfaces.Repository;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Codebridge.Application.CQRS.Dogs.Queries.GetAllDogs
{
    public class GetAllDogsQueryHandler:IRequestHandler<GetAllDogsQuery,Result<List<DogDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDogsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<DogDto>>> Handle(GetAllDogsQuery query,CancellationToken cancellationToken)
        {

            var dogs = await _unitOfWork.Repository<Dog>().Entities
                   .ProjectTo<DogDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<DogDto>>.SuccessAsync(dogs);
        }









    }
}
