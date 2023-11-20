import type Objective from './objective';

export default interface ObjectiveCollection {
    id: string;
    name: string;
    objectives: Objective[];
}