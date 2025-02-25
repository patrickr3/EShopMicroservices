
namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        string message = $"DeleteProductHandler.Handle called with {command}";
        logger.LogInformation(message);
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
