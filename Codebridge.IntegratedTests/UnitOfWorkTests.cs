using Codebridge.Domain.Entities;
using Codebridge.Persistant.Data.Contexts;
using Codebridge.Persistant.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Codebridge.IntegratedTests
{
    public class UnitOfWorkTests
    {
        private readonly DbContextOptions<CodebridgeContext> _options;
        private CodebridgeContext _context;
        private UnitOfWork _unitOfWork;
        private readonly Mock<IMemoryCache> _mockMemoryCache;

        public UnitOfWorkTests()
        {
            _options = new DbContextOptionsBuilder<CodebridgeContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _mockMemoryCache = new Mock<IMemoryCache>();

        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity_WhenEntityDoesNotExist()
        {
            var testEntity = new Dog { Id = Guid.NewGuid(),Colors = "a",Name = "Bd",CreatedDate = DateTime.Now,TailLength = 3,Weight = 20 };

            var repository = _unitOfWork.Repository<Dog>();
            var result = await repository.AddAsync(testEntity);

            await _unitOfWork.Save(CancellationToken.None);  

            var addedEntity = await _context.Set<Dog>().FindAsync(testEntity.Id);
            Assert.NotNull(addedEntity);
            Assert.Equal(testEntity.Id,addedEntity.Id);  
        }


        [Fact]
        public void Repository_ShouldCreateRepository_WhenRepositoryDoesNotExist()
        {

            var repository = _unitOfWork.Repository<Dog>();

            Assert.NotNull(repository);
            Assert.IsType<GenericRepository<Dog>>(repository);
        }



        [Fact]
        public async Task Rollback_ShouldUndoChanges()
        {
            _context = new CodebridgeContext(_options);
            _unitOfWork = new UnitOfWork(_context,new MemoryCache(new MemoryCacheOptions()));

            var entity = new Dog { Id = Guid.NewGuid(),Colors = "a",Name = "Bd",CreatedDate = DateTime.Now,TailLength = 3,Weight = 20 };
            _context.Dogs.Add(entity);
            await _context.SaveChangesAsync();

            entity.Name = "ChangedValue";
           await  _unitOfWork.Repository<Dog>().UpdateAsync(entity);

            await _unitOfWork.Rollback();

            var reloadedEntity = _unitOfWork.Repository<Dog>().GetByIdAsync(entity.Id).Result;

            Assert.NotEqual("ChangedValue",reloadedEntity.Name);
        }




        [Fact]
        public async Task Save_ShouldSetCreatedDateForAddedEntities()
        {

            var entity = new Dog { Id = Guid.NewGuid(),Colors = "a",Name = "Bd",CreatedDate = DateTime.Now,TailLength = 3,Weight = 20 };
            await _unitOfWork.Repository<Dog>().AddAsync(entity);

            await _unitOfWork.Save(CancellationToken.None);

            Assert.NotNull(entity.CreatedDate);
        }

        [Fact]
        public async Task Save_ShouldSetUpdatedDateForModifiedEntities()
        {

            var entity = new Dog { Id = Guid.NewGuid(),Colors = "a",Name = "Bd",CreatedDate = DateTime.Now,TailLength = 3,Weight = 20 };
            _context.Dogs.Add(entity);
            await _context.SaveChangesAsync();

            entity.Name = "ChangedValue";
            await  _unitOfWork.Repository<Dog>().UpdateAsync(entity);

            await _unitOfWork.Save(CancellationToken.None);

            Assert.NotNull(entity.UpdatedDate);
        }



        [Fact]
        public async Task SaveAndRemoveCache_ShouldSaveChangesAndRemoveCacheKeys()
        {
            var entity = new Dog { Id = Guid.NewGuid(),Colors = "a",Name = "Bd",CreatedDate = DateTime.Now,TailLength = 3,Weight = 20 };
            _context.Dogs.Add(entity);

            string[] cacheKeys = { "key1","key2" };
            _mockMemoryCache.Setup(m => m.Remove(It.IsAny<object>()));

            var changesCount = await _unitOfWork.SaveAndRemoveCache(CancellationToken.None,cacheKeys);
            Assert.Equal(1,changesCount);  
            _mockMemoryCache.Verify(m => m.Remove("key1"),Times.Once());
            _mockMemoryCache.Verify(m => m.Remove("key2"),Times.Once());
        }





    }

}
