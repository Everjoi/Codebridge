using Codebridge.Application.Interfaces.Repository;
using Codebridge.Domain.Common;
using Codebridge.Domain.Exceptions;
using Codebridge.Persistant.Data.Contexts;
using Microsoft.EntityFrameworkCore;


namespace Codebridge.Persistant.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly CodebridgeContext _dbContext;

        public GenericRepository(CodebridgeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            if(await _dbContext.Set<T>().AnyAsync(x => x.Id == entity.Id))
                throw new AlreadyExistException(typeof(T),entity.Id);

            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public  Task UpdateAsync(T entity)
        {
            if(!_dbContext.Set<T>().AnyAsync(x => x.Id == entity.Id).Result)
                throw new NotFoundException(typeof(T),entity.Id);

            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            if(!_dbContext.Set<T>().AnyAsync(x => x.Id == entity.Id).Result)
                throw new NotFoundException(typeof(T),entity.Id);

            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var result = await _dbContext.Set<T>().FindAsync(id);

            if(result == null)
                throw new NotFoundException(typeof(T),id);

            return result;
        }
    }
}
