using BingoBlitz_CommunityHub.Data.Interfaces;
using BingoBlitz_CommunityHub.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace BingoBlitz_CommunityHub.Data
{
    /// <summary>
    /// Implements ObjectiveData storage using CosmosDB
    /// </summary>
    public class CosmosObjectiveData : IObjectiveData
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosObjectiveData(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer("CommunityHub", "ObjectiveCollection");
        }

        [Obsolete("Method currently not working as expected, use 'GetAllObjectiveCollections' instead.")]
        public async Task<IterableObjectiveCollectionData> QueryCollectionsByPage(int pageNumber, int pageSize, string? continuationToken = null, string filter = "")
        {
            QueryDefinition query = new QueryDefinition("SELECT * FROM ObjectiveCollection");

            //QueryDefinition query = new QueryDefinition("SELECT * FROM ObjectiveCollection WHERE ObjectiveCollection.Name LIKE @nameFilter")
            //    .WithParameter("@nameFilter", $"%{filter}%");

            FeedIterator<ObjectiveCollection> iterator = _container.GetItemQueryIterator<ObjectiveCollection>(
                query,
                requestOptions: new()
                {
                    MaxItemCount = pageSize
                }
            );

            FeedResponse<ObjectiveCollection> a = await iterator.ReadNextAsync();

            return new(
                continuationToken: a.ContinuationToken,
                totalPages: 5,
                objectiveCollections: a.Resource.ToList()
            );
        }

        public async Task<ObjectiveCollection> GetObjectiveCollectionById(string id)
        {
            ItemResponse<ObjectiveCollection> collection = await _container.ReadItemAsync<ObjectiveCollection>(id, PartitionKey.None);

            return collection.Resource;
        }

        public async Task<string> SaveObjectiveCollection(ObjectiveCollection collection)
        {
            collection.Id = Guid.NewGuid().ToString();

            ItemResponse<ObjectiveCollection> response = await _container.CreateItemAsync(collection);

            return response.Resource.Id;
        }

        public async Task<List<ObjectiveCollection>> GetAllObjectiveCollections()
        {
            IOrderedQueryable<ObjectiveCollection> linq = _container.GetItemLinqQueryable<ObjectiveCollection>();

            FeedResponse<ObjectiveCollection> response = await linq.ToFeedIterator().ReadNextAsync();

            return response.ToList();
        }
    }
}
