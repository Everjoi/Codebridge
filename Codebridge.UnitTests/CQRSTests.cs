using AutoMapper;
using Codebridge.Application.CQRS.Dogs.Commands.CreateDog;
using Codebridge.Application.CQRS.Dogs.Commands.DeleteDog;
using Codebridge.Application.CQRS.Dogs.Commands.UpdateDog;
using Codebridge.Application.CQRS.Dogs.Queries.GetAllDogs;
using Codebridge.Application.DTOs;
using Codebridge.Application.Interfaces.Repository;
using Codebridge.Domain.Entities;
using Codebridge.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Codebridge.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Codebridge.Persistant.Repository;
using Codebridge.Persistant.Data.Contexts;
using Microsoft.Extensions.Caching.Memory;

namespace Codebridge.UnitTests
{
    public class CQRSTests
    { 
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IGenericRepository<Dog>> _repositoryMock;

        public CQRSTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
           _repositoryMock = new Mock<IGenericRepository<Dog>>();
        }

        

        [Fact]
        public async Task CreateDogCommandHandler_ShouldAddDogSuccessfully()
        {

            var dogCommand = new CreateDogCommnd
            {
                Id = Guid.NewGuid(),
                Name = "Neo",
                Colors = "Black",
                TailLength = 5.5,
                Weight = 20.5
            };

            var expectedDog = new Dog
            {
                Id = dogCommand.Id,
                Name = dogCommand.Name,
                Colors = dogCommand.Colors,
                TailLength = dogCommand.TailLength,
                Weight = dogCommand.Weight,
                CreatedDate = DateTime.Now,
                UpdatedDate = null
            };

            _unitOfWorkMock.Setup(uow => uow.Repository<Dog>())
                         .Returns(_repositoryMock.Object);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Dog>()))
                         .ReturnsAsync(expectedDog);

            var handler = new CreateDogCommandHandler(_unitOfWorkMock.Object,_mapperMock.Object);
            var result = await handler.Handle(dogCommand,CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(expectedDog.Id,result.Data);
            _unitOfWorkMock.Verify(uow => uow.Save(It.IsAny<CancellationToken>()),Times.Once());
        }

        [Fact]
        public async Task CreateDogCommandHandler_ShouldPublishDogCreatedEvent()
        {

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IGenericRepository<Dog>>();
            var mapperMock = new Mock<IMapper>();

            var dogCommand = new CreateDogCommnd
            {
                Id = Guid.NewGuid(),
                Name = "Buddy",
                Colors = "Black",
                TailLength = 5.5,
                Weight = 20.5
            };

            Dog createdDog = null;

            repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Dog>()))
                         .Callback<Dog>(d => createdDog = d)
                         .ReturnsAsync(() => createdDog);

            unitOfWorkMock.Setup(uow => uow.Repository<Dog>())
                         .Returns(repositoryMock.Object);

            var handler = new CreateDogCommandHandler(unitOfWorkMock.Object,mapperMock.Object);
            await handler.Handle(dogCommand,CancellationToken.None);
            Assert.Single(createdDog.DomainEvents);
            var domainEvent = createdDog.DomainEvents.First();
            Assert.IsType<DogCreatedEvent>(domainEvent);
            Assert.Equal(createdDog,((DogCreatedEvent)domainEvent).Dog);
        }

        [Fact]
        public async Task Handle_ShouldDeleteDog_WhenDogExists()
        {

           var  _handler = new DeleteDogCommandHandler(_unitOfWorkMock.Object,_mapperMock.Object);

            var dogId = Guid.NewGuid();
            var dog = new Dog { Id = dogId };
            var dogRepoMock = new Mock<IGenericRepository<Dog>>();
            dogRepoMock.Setup(repo => repo.GetByIdAsync(dogId)).ReturnsAsync(dog);
            _unitOfWorkMock.Setup(uow => uow.Repository<Dog>()).Returns(dogRepoMock.Object);

            var command = new DeleteDogCommand(dogId);
            var result = await _handler.Handle(command,CancellationToken.None);

            Assert.Equal(dogId,result.Data);
            _unitOfWorkMock.Verify(uow => uow.Save(It.IsAny<CancellationToken>()),Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateDogSuccessfully_WhenDogExists()
        {
            var dog = new Dog { Id = Guid.NewGuid(),Name = "OldName" };
            var updatedName = "NewName";

            _repositoryMock.Setup(repo => repo.GetByIdAsync(dog.Id)).ReturnsAsync(dog);
            _unitOfWorkMock.Setup(uow => uow.Repository<Dog>()).Returns(_repositoryMock.Object);

            var command = new UpdateDogCommand
            {
                Id = dog.Id,
                Name = updatedName,
            };

            var handler = new UpdateDogCommandHandler(_unitOfWorkMock.Object,_mapperMock.Object);

            var result = await handler.Handle(command,CancellationToken.None);

            Assert.Equal(dog.Id,result.Data);
            Assert.Equal(updatedName,dog.Name);
        }



    }
}
