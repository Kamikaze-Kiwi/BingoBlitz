import type Objective from './objective';

export default interface GameStateItem {
    objective: Objective;
    claimedBy: number | null;
}