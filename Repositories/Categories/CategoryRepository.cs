using AutoMapper;
using MongoDB.Driver;
using PruebaTecnica.Data;
using PruebaTecnica.Entities;
using PruebaTecnica.Models;

namespace PruebaTecnica.Repositories.Categories;

public class CategoryRepository(IStoreContext context, IMapper mapper) : ICategoryRepository
{
  /// <summary>
  /// Creates a new category in the database. </summary> <param name="category">The category information to be created.</param> <returns>The Id of the newly created category.</returns>
  /// /
  public async Task<string> CreateCategory(CategoryDto category)
  {
    var categoryEntity = mapper.Map<Category>(category);

    await context.Store.InsertOneAsync(categoryEntity);

    return categoryEntity.Id;
  }

  /// <summary>
  /// Retrieves a category by its ID.
  /// </summary>
  /// <param name="categoryId">The ID of the category to retrieve.</param>
  /// <returns>The category with the specified ID.</returns>
  public async Task<Category> GetCategory(string categoryId)
  {
    var cursor = await context.Store.FindAsync(c => c.Id == categoryId);
    return await cursor.FirstOrDefaultAsync();
  }

  /// <summary>
  /// Retrieves all categories from the store.
  /// </summary>
  /// <returns>List of Category objects.</returns>
  public async Task<List<Category>> GetCategories()
  {
    var cursor = await context.Store.FindAsync(c => true);
    return await cursor.ToListAsync();
  }

  /// <summary>
  /// Updates the name of a category with the specified identifier.
  /// </summary>
  /// <param name="categoryId">The identifier of the category to update.</param>
  /// <param name="name">The new name of the category.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public async Task UpdateCategory(string categoryId, string name)
  {
    var update = Builders<Category>.Update.Set(c => c.Name, name);

    await context.Store.UpdateOneAsync(c => c.Id == categoryId, update);
  }

  /// <summary>
  /// Deletes a category from the store.
  /// </summary>
  /// <param name="categoryId">The ID of the category to delete.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public async Task DeleteCategory(string categoryId)
  {
    await context.Store.DeleteOneAsync(c => c.Id == categoryId);
  }

  public async Task AddSubCategory(string parentCategoryId, SubCategoryDto subCategory, bool hasProducts,
    bool hasSubCategories)
  {
    var subCategoryEntity = mapper.Map<SubCategory>(subCategory);
    subCategoryEntity.Products = hasProducts ? [] : null;
    subCategoryEntity.ChildSubCategories = hasSubCategories ? [] : null;

    if (string.IsNullOrEmpty(subCategory.ParentSubCategoryId))
    {
      var filter = Builders<Category>.Filter.Eq(c => c.Id, parentCategoryId);
      var update = Builders<Category>.Update.Push(c => c.SubCategories, subCategoryEntity);

      await context.Store.UpdateOneAsync(filter, update);
    }
    else
    {
      var category = await GetCategory(parentCategoryId);
      var parentSubCategory = FindSubCategory(category, subCategory.ParentSubCategoryId);

      if (parentSubCategory == null)
      {
        throw new Exception("Parent subcategory not found");
      }

      parentSubCategory.ChildSubCategories.Add(subCategoryEntity);

      var filter = Builders<Category>.Filter.Eq(c => c.Id, parentCategoryId);
      var update = Builders<Category>.Update.Set(c => c.SubCategories, category.SubCategories);

      await context.Store.UpdateOneAsync(filter, update);
    }
  }

  /// <summary>
  /// Updates the name of a subcategory within a parent category. </summary> <param name="parentCategoryId">The ID of the parent category.</param> <param name="subCategoryId">The ID of the subcategory to update.</param> <param name="name">The new name for the subcategory.</param> <returns>A task representing the asynchronous operation.</returns> <exception cref="System.Exception">Thrown when the subcategory is not found.</exception>
  /// /
  public async Task UpdateSubCategory(string parentCategoryId, string subCategoryId, string name)
  {
    var category = await GetCategory(parentCategoryId);
    var subCategoryToUpdate = FindSubCategory(category, subCategoryId);

    if (subCategoryToUpdate == null)
    {
      throw new Exception("Subcategory not found");
    }

    subCategoryToUpdate.Name = name;

    var filter = Builders<Category>.Filter.Eq(c => c.Id, parentCategoryId);
    var update = Builders<Category>.Update.Set(c => c.SubCategories, category.SubCategories);

    await context.Store.UpdateOneAsync(filter, update);
  }

  /// <summary>
  /// Deletes a subcategory from a parent category.
  /// </summary>
  /// <param name="parentCategoryId">The ID of the parent category.</param>
  /// <param name="subCategoryId">The ID of the subcategory to be deleted.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public async Task DeleteSubCategory(string parentCategoryId, string subCategoryId)
  {
    var category = await GetCategory(parentCategoryId);
    var parentSubCategory = FindParentSubCategory(category, subCategoryId);

    if (parentSubCategory == null)
    {
      throw new Exception("Parent subcategory not found");
    }

    parentSubCategory.ChildSubCategories.RemoveAll(sc => sc.Id == subCategoryId);

    var filter = Builders<Category>.Filter.Eq(c => c.Id, parentCategoryId);
    var update = Builders<Category>.Update.Set(c => c.SubCategories, category.SubCategories);

    await context.Store.UpdateOneAsync(filter, update);
  }

  /// <summary>
  /// Finds a subcategory with the specified ID in a category.
  /// </summary>
  /// <param name="category">The category to search in.</param>
  /// <param name="subCategoryId">The ID of the subcategory to find.</param>
  /// <returns>The subcategory with the specified ID, if found; otherwise, null.</returns>
  public SubCategory FindSubCategory(Category category, string subCategoryId)
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

  /// <summary>
  /// Recursively searches for a subcategory by its ID within a given parent subcategory.
  /// </summary>
  /// <param name="parentSubCategory">The parent subcategory to search within.</param>
  /// <param name="subCategoryId">The ID of the subcategory to find.</param>
  /// <returns>
  /// The subcategory with the specified ID if found, otherwise null.
  /// </returns>
  private static SubCategory FindSubCategory(SubCategory parentSubCategory, string subCategoryId)
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

  /// <summary>
  /// This method finds the parent subcategory based on the provided category and subcategory ID.
  /// </summary>
  /// <param name="category">The category containing subcategories.</param>
  /// <param name="subCategoryId">The ID of the subcategory to find the parent for.</param>
  /// <returns>The parent subcategory if found; otherwise, null.</returns>
  private SubCategory FindParentSubCategory(Category category, string subCategoryId)
  {
    foreach (var subCategory in category.SubCategories)
    {
      if (subCategory.ChildSubCategories.Any(sc => sc.Id == subCategoryId))
      {
        return subCategory;
      }

      var foundParentSubCategory = FindParentSubCategory(subCategory, subCategoryId);

      if (foundParentSubCategory != null)
      {
        return foundParentSubCategory;
      }
    }

    return null;
  }

  /// <summary>
  /// Recursively finds the parent subcategory that contains the subcategory with the specified ID.
  /// </summary>
  /// <param name="parentSubCategory">The parent subcategory to start the search from.</param>
  /// <param name="subCategoryId">The ID of the subcategory to find.</param>
  /// <returns>
  /// The parent subcategory that contains the subcategory with the specified ID, or null if not found.
  /// </returns>
  private SubCategory FindParentSubCategory(SubCategory parentSubCategory, string subCategoryId)
  {
    foreach (var subCategory in parentSubCategory.ChildSubCategories)
    {
      if (subCategory.ChildSubCategories.Any(sc => sc.Id == subCategoryId))
      {
        return parentSubCategory;
      }

      var foundParentSubCategory = FindParentSubCategory(subCategory, subCategoryId);

      if (foundParentSubCategory != null)
      {
        return foundParentSubCategory;
      }
    }

    return null;
  }
}