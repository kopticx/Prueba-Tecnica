using PruebaTecnica.Entities;
using PruebaTecnica.Models;

namespace PruebaTecnica.Repositories.Categories;

public interface ICategoryRepository
{
  Task<Category> GetCategory(string categoryId);
  Task<string> CreateCategory(CategoryDto category);
  Task UpdateCategory(string categoryId, string name);
  Task DeleteCategory(string categoryId);
  Task AddSubCategory(string parentCategoryId, SubCategoryDto subCategory, bool hasProducts, bool hasSubCategories);
  Task UpdateSubCategory(string parentCategoryId, string subCategoryId, string name);
  Task DeleteSubCategory(string parentCategoryId, string subCategoryId);
  Task<List<Category>> GetCategories();
}