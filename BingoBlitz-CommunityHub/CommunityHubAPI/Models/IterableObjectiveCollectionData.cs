namespace BingoBlitz_CommunityHub.Models
{
    public record IterableObjectiveCollectionData
    {
        public IterableObjectiveCollectionData(List<ObjectiveCollectionData> objectiveCollections, string continuationToken)
        {
            ObjectiveCollections = objectiveCollections;
            ContinuationToken = continuationToken;
        }

        public List<ObjectiveCollectionData> ObjectiveCollections { get; set; }
        public string ContinuationToken { get; set; }

        public record ObjectiveCollectionData()
        {
            public ObjectiveCollectionData(string id, string name, int objectiveCount): this() 
            {
                Id = id;
                Name = name;
                ObjectiveCount = objectiveCount;
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public int ObjectiveCount { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
        }
    }
}
