using PruebaTecnica.Entities;
using PruebaTecnica.Models;

namespace PruebaTecnica.Repositories.Products;

public interface IProductRepository
{
  Task<Product> GetProduct(string categoryId, string productId);
  Task AddProductToSubCategory(string categoryId, string subCategoryId, ProductDto productDto);
  Task RemoveProductFromSubCategory(string categoryId, string subCategoryId, string productId);

  Task UpdateProductInSubCategory(string categoryId, string subCategoryId, string productId, string newName,
    int? newStock);
}