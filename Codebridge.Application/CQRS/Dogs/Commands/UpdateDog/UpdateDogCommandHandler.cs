using AutoMapper;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Codebridge.Application.CQRS.Dogs.Commands.UpdateDog
{
    public class UpdateDogCommandHandler:IRequestHandler<UpdateDogCommand,Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDogCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<Guid>> Handle(UpdateDogCommand command,CancellationToken cancellationToken)
        {
            var dog = await _unitOfWork.Repository<Dog>().GetByIdAsync(command.Id);           
              dog.TailLength = command.TailLength;
              dog.Weight = command.Weight;
              dog.Colors = command.Colors;
              dog.Name = command.Name;
              dog.Id = command.Id;
              dog.UpdatedDate = DateTime.Now;

              await _unitOfWork.Repository<Dog>().UpdateAsync(dog);
              dog.AddDomainEvent(new DogUpdatedEvent(dog));

              await _unitOfWork.Save(cancellationToken);

              return await Result<Guid>.SuccessAsync(dog.Id,"Dog Updated.");
        }
    }
}
