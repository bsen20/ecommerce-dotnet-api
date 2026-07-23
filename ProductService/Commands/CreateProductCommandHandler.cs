using MediatR;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly AppDbContext _context;

    public CreateProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Product.Name,
            Description = request.Product.Description,
            Price = request.Product.Price,
            Category = request.Product.Category,
            StockQuantity = request.Product.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            StockQuantity = product.StockQuantity
        };
    }
}