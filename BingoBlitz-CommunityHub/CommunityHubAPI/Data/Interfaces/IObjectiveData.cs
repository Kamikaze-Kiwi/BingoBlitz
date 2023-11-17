using BingoBlitz_CommunityHub.Models;

namespace BingoBlitz_CommunityHub.Data.Interfaces
{
    public interface IObjectiveData
    {
        public Task<IterableObjectiveCollectionData> QueryCollectionsByPage(int pageNumber, int pageSize, string? continuationToken = null, string filter = "");

        public Task<ObjectiveCollection> GetObjectiveCollectionById(string id);

        public Task<string> SaveObjectiveCollection(ObjectiveCollection collection);
        public Task<List<ObjectiveCollection>> GetAllObjectiveCollections();
    }
}
