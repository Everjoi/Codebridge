using Codebridge.Application.Interfaces.Repository;
using Codebridge.Domain.Entities;
using Codebridge.Domain.Exceptions;
using Codebridge.Persistant.Data.Contexts;
using Codebridge.Persistant.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Codebridge.IntegratedTests
{
    public class RepositoryTests
    {
        private DbContextOptions<CodebridgeContext> _options;
        private CodebridgeContext _context;
        private IGenericRepository<Dog> _repository;

        public RepositoryTests()
        {
            _options = new DbContextOptionsBuilder<CodebridgeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

             _context =  new CodebridgeContext(_options);
            _repository = new GenericRepository<Dog>(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenEntityExists()
        {
            var testEntity = new Dog { Id = Guid.NewGuid(),Colors = "a",Name = "Bd",CreatedDate = DateTime.Now,TailLength = 3,Weight = 20 };
            _context.Add(testEntity);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(testEntity.Id);

            Assert.NotNull(result);
            Assert.Equal(testEntity.Id,result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsNotFoundException_WhenEntityDoesNotExist()
        {
            await Assert.ThrowsAsync<NotFoundException>(() => _repository.GetByIdAsync(Guid.NewGuid()));
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {

            var testEntities = new List<Dog>
            {
                 new Dog { Id=Guid.NewGuid(),Colors = "d", TailLength = 13, Weight =40 },
                 new Dog {Id=Guid.NewGuid(),Colors = "a", TailLength = 11, Weight =42 },
                 new Dog { Id=Guid.NewGuid(),Colors = "f", TailLength = 3, Weight =17 }
            };

            _context.AddRange(testEntities);
            await _context.SaveChangesAsync();


            var results = await _repository.GetAllAsync();

            Assert.NotNull(results);
            Assert.Equal(testEntities.Count,results.Count);

            foreach(var entity in testEntities)
            {
                Assert.Contains(entity,results);
            }
        }


        [Fact]
        public async Task DeleteAsync_DeletesEntity_WhenEntityExists()
        {

            var testEntity = new Dog { Id = Guid.NewGuid(),Colors = "a",TailLength = 63,Weight = 53 };
            _context.Add(testEntity);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(testEntity);
            await _context.SaveChangesAsync();

            var deletedEntity = await _context.Set<Dog>().FindAsync(testEntity.Id);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsNotFoundException_WhenEntityDoesNotExist()
        {

            var nonExistentEntity = new Dog { Id = Guid.NewGuid(),Colors = "s",Weight = 23,Name = "Dfg",TailLength = 23 };

            await Assert.ThrowsAsync<NotFoundException>(() => _repository.DeleteAsync(nonExistentEntity));
        }



        [Fact]
        public async Task UpdateAsync_UpdatesEntity_WhenEntityExists()
        {

            var testEntity = new Dog { Id = Guid.NewGuid(),Colors = "s",Weight = 23,Name = "Dfg",TailLength = 23 };
            _context.Add(testEntity);
            await _context.SaveChangesAsync();

            testEntity.Name = "NewValue";

            await _repository.UpdateAsync(testEntity);
            await _context.SaveChangesAsync();

            var updatedEntity = await _context.Set<Dog>().FindAsync(testEntity.Id);
            Assert.Equal("NewValue",updatedEntity.Name);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsNotFoundException_WhenEntityDoesNotExist()
        {
            var nonExistentEntity = new Dog { Id = Guid.NewGuid(),Colors = "s",Weight = 23,Name = "Dfg",TailLength = 23 };

            await Assert.ThrowsAsync<NotFoundException>(() => _repository.UpdateAsync(nonExistentEntity));
        }





        [Fact]
        public async Task AddAsync_AddsEntity_WhenEntityDoesNotExist()
        {
            var testEntity = new Dog { Id = Guid.NewGuid(),Colors = "s",Weight = 23,Name = "Dfg",TailLength = 23 };

            var result = await _repository.AddAsync(testEntity);
            await _context.SaveChangesAsync();
            var addedEntity = await _context.Set<Dog>()
                .FindAsync(testEntity.Id);
            Assert.NotNull(addedEntity);
            Assert.Equal(testEntity.Id,addedEntity.Id);
            
        }

         [Fact] public async Task AddAsync_ThrowsAlreadyExistException_WhenEntityExists() 
         { 
            var testEntity = new Dog { Id = Guid.NewGuid(),Colors = "s",Weight = 23,Name = "Dfg",TailLength = 23 };
            await _context.AddAsync(testEntity);
            await _context.SaveChangesAsync(); 
            var duplicateEntity = new Dog { Id = testEntity.Id };     
            Assert.ThrowsAsync<AlreadyExistException>(() => _repository.AddAsync(duplicateEntity));
         }


    }
}
