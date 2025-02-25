using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"GetProductsQueryHandler.Handle call with {query}");
        var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);
        return new GetProductsByCategoryResult(products);
    }
}
