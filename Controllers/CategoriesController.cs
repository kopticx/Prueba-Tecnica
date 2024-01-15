using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models;
using PruebaTecnica.Repositories.Categories;

namespace PruebaTecnica.Controllers;

/// <summary>
/// Controller class for handling category related operations.
/// </summary>
[ApiController]
[Route("api/categories")]
public class CategoriesController(ICategoryRepository repository) : ControllerBase
{
  [HttpGet("GetCategories")]
  public async Task<IActionResult> GetCategories()
  {
    var categories = await repository.GetCategories();

    return Ok(categories);
  }

  /// <summary>
  /// Retrieves a category based on the provided category ID.
  /// </summary>
  /// <param name="categoryId">The ID of the category to retrieve.</param>
  /// <returns>An <see cref="IActionResult"/> representing the HTTP response that contains the retrieved category if found; otherwise, a 404 Not Found response.</returns>
  [HttpGet]
  public async Task<IActionResult> GetCategory(string categoryId)
  {
    var category = await repository.GetCategory(categoryId);

    return Ok(category);
  }

  /// <summary>
  /// Creates a new category.
  /// </summary>
  /// <param name="category">The category data.</param>
  /// <returns>An IActionResult representing the status of the operation.</returns>
  [HttpPost]
  public async Task<IActionResult> CreateCategory(CategoryDto category)
  {
    var categoryId = await repository.CreateCategory(category);

    return CreatedAtAction(nameof(GetCategory), new { categoryId }, categoryId);
  }

  /// <summary>
  /// Updates a category in the repository.
  /// </summary>
  /// <param name="categoryId">The identifier of the category to update.</param>
  /// <param name="name">The new name of the category.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPut("{categoryId}")]
  public async Task<IActionResult> UpdateCategory(string categoryId, string name)
  {
    await repository.UpdateCategory(categoryId, name);

    return NoContent();
  }

  /// <summary>
  /// Deletes a category from the repository.
  /// </summary>
  /// <param name="categoryId">The ID of the category to delete.</param>
  /// <returns>An IActionResult representing the result of the deletion.</returns>
  [HttpDelete("{categoryId}")]
  public async Task<IActionResult> DeleteCategory(string categoryId)
  {
    await repository.DeleteCategory(categoryId);

    return NoContent();
  }

  /// <summary>
  /// Adds a subcategory to the specified category.
  /// </summary>
  /// <param name="categoryId">The ID of the category to add the subcategory to.</param>
  /// <param name="subCategory">The <see cref="SubCategoryDto"/> object representing the subcategory to be added.</param>
  /// <param name="hasProducts">A boolean value indicating if the subcategory has products.</param>
  /// <param name="hasSubCategories">A boolean value indicating if the subcategory has subcategories.</param>
  /// <returns>
  /// A <see cref="Task{IActionResult}"/> representing the asynchronous operation.
  /// The task result contains an <see cref="ActionResult"/> object representing the created subcategory.
  /// </returns>
  [HttpPost("addSubCategory/{categoryId}")]
  [ProducesResponseType((int)HttpStatusCode.Created)]
  public async Task<IActionResult> AddSubCategory(string categoryId, SubCategoryDto subCategory, bool hasProducts,
    bool hasSubCategories)
  {
    await repository.AddSubCategory(categoryId, subCategory, hasProducts, hasSubCategories);

    return CreatedAtAction(nameof(GetCategory), new { categoryId }, categoryId);
  }

  /// <summary>
  /// Updates a subcategory with the specified category ID, subcategory ID, and name.
  /// </summary>
  /// <param name="categoryId">The ID of the category.</param>
  /// <param name="subCategoryId">The ID of the subcategory.</param>
  /// <param name="name">The new name of the subcategory.</param>
  /// <returns>An IActionResult representing the result of the operation.</returns>
  [HttpPut("{categoryId}/UpdateSubCategory/{subCategoryId}")]
  public async Task<IActionResult> UpdateSubCategory(string categoryId, string subCategoryId, string name)
  {
    await repository.UpdateSubCategory(categoryId, subCategoryId, name);

    return NoContent();
  }

  /// <summary>
  /// Deletes a subcategory from a category.
  /// </summary>
  /// <param name="categoryId">The ID of the category.</param>
  /// <param name="subCategoryId">The ID of the subcategory.</param>
  /// <returns>An IActionResult representing the status of the delete operation.</returns>
  /// <example>
  /// This example shows how to delete a subcategory.
  /// <code>
  /// string categoryId = "category1";
  /// string subCategoryId = "subcategory1";
  /// IActionResult result = await DeleteSubCategory(categoryId, subCategoryId);
  /// </code>
  /// </example>
  [HttpDelete("{categoryId}/DeleteSubCategory/{subCategoryId}")]
  public async Task<IActionResult> DeleteSubCategory(string categoryId, string subCategoryId)
  {
    await repository.DeleteSubCategory(categoryId, subCategoryId);

    return NoContent();
  }
}