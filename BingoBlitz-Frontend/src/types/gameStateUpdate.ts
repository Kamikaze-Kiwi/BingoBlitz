export interface GameStateUpdate {
    type: GameStateUpdateType;
    data: any;
}

export enum GameStateUpdateType {
    itemClaimed = 'itemClaimed',
}