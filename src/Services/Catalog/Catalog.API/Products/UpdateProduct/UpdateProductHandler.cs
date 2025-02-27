namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
  public UpdateProductCommandValidator()
  {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Product Id is required");
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Name is required")
      .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
    RuleFor(x => x.Price)
      .GreaterThan(0).WithMessage("Price must be greater than 0");
  }
}


internal class UpdateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand query, CancellationToken cancellationToken)
    {
        Product product = await session.LoadAsync<Product>(query.Id, cancellationToken) ?? throw new ProductNotFoundException(query.Id);
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
