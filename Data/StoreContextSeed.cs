using MongoDB.Bson;
using MongoDB.Driver;
using PruebaTecnica.Entities;

namespace PruebaTecnica.Data;

public class StoreContextSeed
{
  public static void SeedData(IMongoCollection<Category> storeCollection)
  {
    var existProduct = storeCollection.Find(p => true).Any();
    if (!existProduct)
    {
      storeCollection.InsertManyAsync(GetPreconfiguredCategories());
    }
  }

  private static IEnumerable<Category> GetPreconfiguredCategories()
  {
    return new List<Category>
    {
      new()
      {
        Id = "1.1",
        Name = "tecnología",
        SubCategories =
        [
          new SubCategory
          {
            Id = "1.1.1",
            Name = "computación",
            ParentSubCategoryId = null,
            ChildSubCategories =
            [
              new SubCategory
              {
                Id = "1.1.1.1",
                Name = "computadora de escritorio",
                ParentSubCategoryId = "1.1.1",
                ChildSubCategories = null,
                Products = []
              },
              new SubCategory
              {
                Id = "1.1.1.2",
                Name = "computadora portátil",
                ParentSubCategoryId = "1.1.1",
                ChildSubCategories = null,
                Products = [
                  new Product
                  {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Dell 4512",
                    NumMaterial = "AX-4342FD",
                    Stock = 3
                  }
                ]
              },
              new SubCategory
              {
                Id = "1.1.1.3",
                Name = "tablets",
                ParentSubCategoryId = "1.1.1",
                ChildSubCategories = null,
                Products = []
              }
            ],
            Products = null
          },

          new SubCategory
          {
            Id = "1.1.2",
            Name = "telefonía",
            ParentSubCategoryId = null,
            ChildSubCategories =
            [
              new SubCategory
              {
                Id = "1.1.2.1",
                Name = "celular",
                ParentSubCategoryId = "1.1.2",
                ChildSubCategories = null,
                Products = [
                  new Product
                  {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Iphone X",
                    NumMaterial = "AD-4332EE",
                    Stock = 10
                  }
                ]
              },
              new SubCategory
              {
                Id = "1.1.2.2",
                Name = "accesorios",
                ParentSubCategoryId = "1.1.2",
                ChildSubCategories = null,
                Products = [
                  new Product
                  {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Correa",
                    NumMaterial = "AC-5545Q",
                    Stock = 0
                  }
                ]
              }
            ],
            Products = null
          }
        ]
      },
      new()
      {
        Id = "1.2",
        Name = "farmacia",
        SubCategories =
        [
          new SubCategory
          {
            Id = "1.2.1",
            Name = "medicamentos",
            ParentSubCategoryId = null,
            ChildSubCategories =
            [
              new SubCategory
              {
                Id = "1.2.1.1",
                Name = "analgésicos",
                ParentSubCategoryId = "1.2.1",
                ChildSubCategories = null,
                Products = [
                  new Product
                  {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Aspirina",
                    NumMaterial = "MD-7456AS",
                    Stock = 22
                  }
                ]
              },
              new SubCategory
              {
                Id = "1.2.1.2",
                Name = "estomacal",
                ParentSubCategoryId = "1.2.1",
                ChildSubCategories = null,
                Products = []
              }
            ],
            Products = null
          }
        ]
      },
      new()
      {
        Id = "1.3",
        Name = "hogar",
        SubCategories =
        [
          new SubCategory
          {
            Id = "1.3.1",
            Name = "baño",
            ParentSubCategoryId = null,
            ChildSubCategories =
            [
              new SubCategory
              {
                Id = "1.3.1.1",
                Name = "toallas",
                ParentSubCategoryId = "1.3.1",
                ChildSubCategories = null,
                Products = []
              },
              new SubCategory
              {
                Id = "1.3.1.2",
                Name = "batas",
                ParentSubCategoryId = "1.3.1",
                ChildSubCategories = null,
                Products = [
                  new Product
                  {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "Bata hombre",
                    NumMaterial = "BN-18643",
                    Stock = 1
                  }
                ]
              }
            ],
            Products = null
          }
        ]
      }
    };
  }
}