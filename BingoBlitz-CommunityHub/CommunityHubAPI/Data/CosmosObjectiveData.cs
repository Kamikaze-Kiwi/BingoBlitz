using BingoBlitz_CommunityHub.Data.Interfaces;
using BingoBlitz_CommunityHub.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

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

        public async Task<IterableObjectiveCollectionData> QueryCollectionsByPage(int pageSize, string? continuationToken = null, string filter = "")
        {
            QueryRequestOptions requestOptions = new()
            {
                MaxItemCount = pageSize
            };

            QueryDefinition queryDefinition = new QueryDefinition("SELECT c.id, c.Name, ARRAY_LENGTH(c.Objectives) AS ObjectivesCount FROM c WHERE CONTAINS(c.Name, @filter)")
                .WithParameter("@filter", filter);

            FeedIterator<ObjectiveCollection> feedIterator = _container.GetItemQueryIterator<ObjectiveCollection>(queryDefinition, continuationToken, requestOptions);

            FeedResponse<ObjectiveCollection> result = await feedIterator.ReadNextAsync();

            return new IterableObjectiveCollectionData(result.Resource.ToList(), result.ContinuationToken);
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
    }
}
