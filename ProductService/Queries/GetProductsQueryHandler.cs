using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos;

namespace ProductService.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly AppDbContext _context;

    public GetProductsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                StockQuantity = p.StockQuantity
            })
            .ToListAsync(cancellationToken);
    }
}