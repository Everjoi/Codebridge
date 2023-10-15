using AutoMapper;
using Codebridge.Application.DTOs;
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

namespace Codebridge.Application.CQRS.Dogs.Queries.GetDogById
{
    public class GetDogByIdQueryHandler  : IRequestHandler<GetDogByIdQuery,Result<DogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDogByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DogDto>> Handle(GetDogByIdQuery query,CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Dog>().GetByIdAsync(query.Id);
            var dog = _mapper.Map<DogDto>(entity);
            return await Result<DogDto>.SuccessAsync(dog);
        }

    }
}
