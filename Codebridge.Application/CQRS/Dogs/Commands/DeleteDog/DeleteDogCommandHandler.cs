using AutoMapper;
using Codebridge.Application.CQRS.Dogs.Commands.UpdateDog;
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

namespace Codebridge.Application.CQRS.Dogs.Commands.DeleteDog
{
    public class DeleteDogCommandHandler : IRequestHandler<DeleteDogCommand,Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteDogCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(DeleteDogCommand command,CancellationToken cancellationToken)
        {

            var dog = await _unitOfWork.Repository<Dog>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<Dog>().DeleteAsync(dog);
            dog.AddDomainEvent(new DogDeletedEvent(dog));

            await _unitOfWork.Save(cancellationToken);

            return await Result<Guid>.SuccessAsync(dog.Id,"Dog Deleted");
        }
    }
}
