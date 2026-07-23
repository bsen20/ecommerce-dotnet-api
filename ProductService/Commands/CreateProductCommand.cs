using MediatR;
using ProductService.Dtos;

namespace ProductService.Commands;

public record CreateProductCommand(CreateProductDto Product) : IRequest<ProductDto>;