import type ObjectiveCollectionData from './objectiveCollectionData';

export default interface IterableObjectiveCollectionData {
    objectiveCollections: ObjectiveCollectionData[];
    continuationToken: string;  
}