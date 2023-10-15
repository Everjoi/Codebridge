using AutoMapper;
using Codebridge.Application.Interfaces.Repository;
using Codebridge.Application.Mappings;
using Codebridge.Domain.Entities;
using Codebridge.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codebridge.Application.CQRS.Dogs.Commands.CreateDog
{
    public class CreateDogCommandHandler:IRequestHandler<CreateDogCommnd,Result<Guid>> 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateDogCommnd command,CancellationToken cancellationToken)
        {

            var dog = new Dog
            {
                Id = command.Id,
                Colors = command.Colors,
                Name = command.Name,
                TailLength = command.TailLength,
                Weight = command.Weight,
                CreatedDate = DateTime.Now,
                UpdatedDate = null    
            };

            await _unitOfWork.Repository<Dog>().AddAsync(dog);
            dog.AddDomainEvent(new DogCreatedEvent(dog));
            await _unitOfWork.Save(cancellationToken);
            return await Result<Guid>.SuccessAsync(dog.Id,"Dog Created.");
        }
    }
}
