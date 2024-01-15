using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models;
using PruebaTecnica.Repositories.Products;

namespace PruebaTecnica.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductRepository repository) : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetProduct(string categoryId, string productId)
  {
    var product = await repository.GetProduct(categoryId, productId);

    return Ok(product);
  }

  [HttpPost]
  public async Task<IActionResult> CreateProduct(string categoryId, string subcategory, ProductDto productDto)
  {
    await repository.AddProductToSubCategory(categoryId, subcategory, productDto);

    return NoContent();
  }

  [HttpPut]
  public async Task<IActionResult> UpdateProduct(string categoryId, string subcategory, string productId, int? stock,
    string? name)
  {
    await repository.UpdateProductInSubCategory(categoryId, subcategory, productId, name, stock);

    return NoContent();
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteProduct(string categoryId, string subcategory, string productId)
  {
    await repository.RemoveProductFromSubCategory(categoryId, subcategory, productId);

    return NoContent();
  }
}