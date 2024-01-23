import type Objective from './objective';

export default interface ObjectiveCollectionData {
    id: string;
    name: string;
    objectiveCount: number;
    userId: string;
    userName: string;
    objectives: Objective[];
}