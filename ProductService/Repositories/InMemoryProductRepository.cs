using System.Collections.Concurrent;
using ProductService.Models;

namespace ProductService.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private int _nextId = 0;

    public Task<List<Product>> GetAllAsync()
    {
        return Task.FromResult(_products.Values.ToList());
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<Product> AddAsync(Product product)
    {
        product.Id = Interlocked.Increment(ref _nextId);
        _products[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task<Product> UpdateAsync(Product product)
    {
        _products[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task DeleteAsync(int id)
    {
        _products.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}