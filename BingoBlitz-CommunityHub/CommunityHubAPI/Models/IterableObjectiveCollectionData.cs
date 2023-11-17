namespace BingoBlitz_CommunityHub.Models
{
    public record IterableObjectiveCollectionData
    {
        public IterableObjectiveCollectionData(List<ObjectiveCollection> objectiveCollections, string continuationToken, int totalPages)
        {
            ObjectiveCollections = objectiveCollections;
            ContinuationToken = continuationToken;
            TotalPages = totalPages;
        }

        public List<ObjectiveCollection> ObjectiveCollections { get; set; }
        public string ContinuationToken { get; set; }
        public int TotalPages { get; set; }
    }
}
