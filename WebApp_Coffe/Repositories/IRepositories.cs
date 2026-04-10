using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Coffe.Models;

namespace WebApp_Coffe.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
}

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug);
}

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySlugAsync(string slug);
    Task<List<Product>> GetFeaturedAsync(int count);
}

public interface IBlogRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetBySlugAsync(string slug);
}

public interface IStoreRepository : IRepository<Store>
{
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
}
