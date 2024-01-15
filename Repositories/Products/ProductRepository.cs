using AutoMapper;
using MongoDB.Driver;
using PruebaTecnica.Data;
using PruebaTecnica.Entities;
using PruebaTecnica.Models;
using PruebaTecnica.Repositories.Categories;

namespace PruebaTecnica.Repositories.Products;

public class ProductRepository(IStoreContext context, ICategoryRepository categoryRepository, IMapper mapper) : IProductRepository
{
  public async Task<Product> GetProduct(string categoryId, string productId)
  {
    var category = await categoryRepository.GetCategory(categoryId);
    var product = FindProductInSubCategories(category, productId);

    if (product == null)
    {
      throw new Exception("Product not found");
    }

    return product;
  }

  public async Task AddProductToSubCategory(string categoryId, string subCategoryId, ProductDto productDto)
  {
    var category = await categoryRepository.GetCategory(categoryId);
    var subCategory = FindSubCategory(category, subCategoryId);

    if (subCategory == null)
    {
      throw new Exception("Subcategory not found");
    }

    var product = mapper.Map<Product>(productDto);
    subCategory.Products.Add(product);

    var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
    var update = Builders<Category>.Update.Set(c => c.SubCategories, category.SubCategories);

    await context.Store.UpdateOneAsync(filter, update);
  }

  public async Task RemoveProductFromSubCategory(string categoryId, string subCategoryId, string productId)
  {
    var category = await categoryRepository.GetCategory(categoryId);
    var subCategory = FindSubCategory(category, subCategoryId);

    if (subCategory == null)
    {
      throw new Exception("Subcategory not found");
    }

    var product = subCategory.Products.FirstOrDefault(p => p.Id == productId);

    if (product == null)
    {
      throw new Exception("Product not found");
    }

    subCategory.Products.Remove(product);

    var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
    var update = Builders<Category>.Update.Set(c => c.SubCategories, category.SubCategories);

    await context.Store.UpdateOneAsync(filter, update);
  }

  public async Task UpdateProductInSubCategory(string categoryId, string subCategoryId, string productId, string? newName, int? newStock)
  {
    var category = await categoryRepository.GetCategory(categoryId);
    var subCategory = FindSubCategory(category, subCategoryId);

    if (subCategory == null)
    {
      throw new Exception("Subcategory not found");
    }

    var product = subCategory.Products.FirstOrDefault(p => p.Id == productId);

    if (product == null)
    {
      throw new Exception("Product not found");
    }

    if (newName is not null)
    {
      product.Name = newName;
    }

    if(newStock is not null)
    {
      product.Stock = newStock.Value;
    }

    var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
    var update = Builders<Category>.Update.Set(c => c.SubCategories, category.SubCategories);

    await context.Store.UpdateOneAsync(filter, update);
  }

  private Product FindProductInSubCategories(Category category, string productId)
  {
    foreach (var subCategory in category.SubCategories)
    {
      var product = subCategory.Products.FirstOrDefault(p => p.Id == productId);

      if (product != null)
      {
        return product;
      }

      product = FindProductInSubCategories(subCategory, productId);

      if (product != null)
      {
        return product;
      }
    }

    return null;
  }

  private Product FindProductInSubCategories(SubCategory subCategory, string productId)
  {
    foreach (var childSubCategory in subCategory.ChildSubCategories)
    {
      var product = childSubCategory.Products.FirstOrDefault(p => p.Id == productId);

      if (product != null)
      {
        return product;
      }

      product = FindProductInSubCategories(childSubCategory, productId);

      if (product != null)
      {
        return product;
      }
    }

    return null;
  }

  private SubCategory FindSubCategory(Category category, string subCategoryId)
  {
    foreach (var subCategory in category.SubCategories)
    {
      if (subCategory.Id == subCategoryId)
      {
        return subCategory;
      }

      var foundSubCategory = FindSubCategory(subCategory, subCategoryId);

      if (foundSubCategory != null)
      {
        return foundSubCategory;
      }
    }

    return null;
  }

  private SubCategory FindSubCategory(SubCategory parentSubCategory, string subCategoryId)
  {
    foreach (var subCategory in parentSubCategory.ChildSubCategories)
    {
      if (subCategory.Id == subCategoryId)
      {
        return subCategory;
      }

      var foundSubCategory = FindSubCategory(subCategory, subCategoryId);

      if (foundSubCategory != null)
      {
        return foundSubCategory;
      }
    }

    return null;
  }
}