using MongoDB.Driver;
using PruebaTecnica.Entities;

namespace PruebaTecnica.Data;

public interface IStoreContext
{
  IMongoCollection<Category> Store { get; }
}