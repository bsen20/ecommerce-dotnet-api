using MediatR;
using ProductService.Dtos;

namespace ProductService.Queries;

public record GetProductsQuery : IRequest<List<ProductDto>>;