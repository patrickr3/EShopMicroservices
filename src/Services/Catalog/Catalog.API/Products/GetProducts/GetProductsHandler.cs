﻿namespace Catalog.API.Products.GetProducts;

public record GetProducstQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProducstQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProducstQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
          .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProductsResult(products);
    }
}
