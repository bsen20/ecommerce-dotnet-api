using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Repositories;
using MediatR;
using ProductService.Dtos;
using ProductService.Commands;
using ProductService.Queries;

namespace ProductService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    private readonly AppDbContext _context;

    // DI: repository is injected via constructor
    public ProductsController(IProductRepository repository,AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        var created = await _repository.AddAsync(product);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(int id, Product product)
    {
        product.Id = id;
        var updated = await _repository.UpdateAsync(product);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Product>>> Search(
        [FromQuery] string? name,
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        var products = await query.OrderBy(p => p.Name).ToListAsync();
        return Ok(products);
    }

    [HttpGet("grouped")]
    public async Task<ActionResult> GetGrouped()
    {
        var grouped = await _context.Products
            .GroupBy(p => p.Category)
            .Select(g => new { Category = g.Key, Count = g.Count(), TotalValue = g.Sum(p => p.Price) })
            .ToListAsync();

        return Ok(grouped);
    }
}

/*
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var query = new GetProductsQuery();
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
    {
        var command = new CreateProductCommand(dto);
        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
    }
}*/