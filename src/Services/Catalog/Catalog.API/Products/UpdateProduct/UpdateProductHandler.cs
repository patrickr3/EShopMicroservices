namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);


internal class UpdateProductCommandHandler
    (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"UpdateProductHandler.Handle call with {query}");
        Product product = await session.LoadAsync<Product>(query.Id, cancellationToken) ?? throw new ProductNotFoundException();
        product.Name = query.Name;
        product.Category = query.Category;
        product.Description = query.Description;
        product.ImageFile = query.ImageFile;
        product.Price = query.Price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
}
