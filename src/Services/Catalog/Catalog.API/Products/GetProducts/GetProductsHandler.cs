
namespace Catalog.API.Products.GetProducts;

public record GetProducstQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProducstQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProducstQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle call with {@Query}", query);
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}
