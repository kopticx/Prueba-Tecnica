using MongoDB.Driver;
using PruebaTecnica.Entities;

namespace PruebaTecnica.Data;

public class StoreContext : IStoreContext
{
  public IMongoCollection<Category> Store { get; }

  public StoreContext(IConfiguration configuration)
  {
    var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

    Store = database.GetCollection<Category>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
    StoreContextSeed.SeedData(Store);
  }
}