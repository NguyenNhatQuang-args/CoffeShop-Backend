using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp_Coffe.Data;
using WebApp_Coffe.Models;

namespace WebApp_Coffe.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly CoffeeShopDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(CoffeeShopDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CoffeeShopDbContext context) : base(context) { }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(c => c.Products.Where(p => p.IsActive))
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CoffeeShopDbContext context) : base(context) { }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.ProductVariants)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<List<Product>> GetFeaturedAsync(int count)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.ProductVariants)
            .Include(p => p.ProductTags)
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}

public class BlogRepository : Repository<BlogPost>, IBlogRepository
{
    public BlogRepository(CoffeeShopDbContext context) : base(context) { }

    public async Task<BlogPost?> GetBySlugAsync(string slug)
    {
        return await _dbSet.FirstOrDefaultAsync(b => b.Slug == slug);
    }
}

public class StoreRepository : Repository<Store>, IStoreRepository
{
    public StoreRepository(CoffeeShopDbContext context) : base(context) { }
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(CoffeeShopDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }
}
