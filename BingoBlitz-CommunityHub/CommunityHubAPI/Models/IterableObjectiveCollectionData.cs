namespace BingoBlitz_CommunityHub.Models
{
    public record IterableObjectiveCollectionData
    {
        public IterableObjectiveCollectionData(List<ObjectiveCollection> objectiveCollections, string continuationToken)
        {
            ObjectiveCollections = objectiveCollections;
            ContinuationToken = continuationToken;
        }

        public List<ObjectiveCollection> ObjectiveCollections { get; set; }
        public string ContinuationToken { get; set; }
    }
}
