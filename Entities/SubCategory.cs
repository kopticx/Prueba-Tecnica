namespace PruebaTecnica.Entities;

public class SubCategory
{
  public string Id { get; set; }
  public string Name { get; set; }
  public string ParentSubCategoryId { get; set; }
  public List<SubCategory> ChildSubCategories { get; set; } = [];
  public List<Product> Products { get; set; } = [];
}