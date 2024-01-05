<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import { useRoute } from 'vue-router';
    import * as Ably from 'ably';
    import type GameState from '@/types/gameState';
    import type GameStateItem from '@/types/gameStateItem';
    import { GameStateUpdateType, type GameStateUpdate } from '@/types/gameStateUpdate';

    let gameState = ref({} as GameState);
    let selectedCell = ref({} as GameStateItem | null);
    selectedCell.value = null;

    const teams = [
        { name: 'placeholder', color: 'placeholder'},
        { name: 'Red', color: '#ff0000', players: [] as string[]},
        { name: 'Blue', color: '#0000ff', players: [] as string[] },
        { name: 'Green', color: '#00ff00', players: [] as string[] },
        { name: 'Yellow', color: '#ffff00', players: [] as string[] },
        { name: 'Orange', color: '#ffa500', players: [] as string[] },
        { name: 'Pink', color: '#ffc0cb', players: [] as string[] },
    ];

    //temporary for testing, these values will be retrieved from the server later
    gameState.value.items = [
        [
            { objective: { name: 'Obtain a cup of coffee', description: 'Hold any cup/mug with coffee in it.', thumbnailEmoji: '☕' }, claimedBy: null },
            { objective: { name: 'High-five 10 people', description: 'High-five 10 different people.', thumbnailEmoji: '👏' }, claimedBy: null },
            { objective: { name: 'Throw away some trash', description: 'Throw any piece of trash into a trashcan.', thumbnailEmoji: '🚮' }, claimedBy: null },
        ],
        [
            { objective: { name: 'Touch an animal', description: 'Touch any living animal. Humans and insects do not count.', thumbnailEmoji: '🐶' }, claimedBy: null },
            { objective: { name: 'Get a vehicle to honk', description: 'Make any vehicle honk their horn. Honking a horn yourself does not count.', thumbnailEmoji: '📣' }, claimedBy: null },
            { objective: { name: 'Climb a tree', description: 'Climb any tree. You need to be atleast half a meter of the ground.', thumbnailEmoji: '🌲' }, claimedBy: null },
        ],
        [
            { objective: { name: 'Do a bottle flip', description: 'Flip any bottle and make it land.', thumbnailEmoji: '🍾' }, claimedBy: null },
            { objective: { name: 'Spin 25 circles on a chair', description: 'Spin 25 circles on an office chair', thumbnailEmoji: '🪑' }, claimedBy: null },
            { objective: { name: 'Climb 50 steps', description: 'Climb 50 steps on any stairs. You may not count the same stair twice.', thumbnailEmoji: '📶' }, claimedBy: null },
        ],
    ];

    const gameId = useRoute().params.id as string;
    let playerTeam = ref(1); 

    const ably = new Ably.Realtime.Promise('QSaqKA.ItBuuQ:qp_nN-UeqeNF4GG4zzrhr-0iSOTkON2EZCZpBqpbCsQ');
    const channel = ably.channels.get('game.' + gameId);

    channel.subscribe('gamestate', (message: any) => {
        HandleIncomingMessage(message.data as GameStateUpdate);
    });

    function HandleIncomingMessage(message: GameStateUpdate) {
        switch (message.type) {
            case GameStateUpdateType.itemClaimed:
                gameState.value.items[message.data.x][message.data.y].claimedBy = message.data.claimedBy;
                break;
        }
    }

    function SendMessage(message: GameStateUpdate){
        channel.publish('gamestate', message);
    }

    function SelectCell(cell: GameStateItem) {
        selectedCell.value = cell;
    }

    function ClaimCell(cell: GameStateItem) {
        cell.claimedBy = playerTeam.value;
        selectedCell.value = null;

        const cellIndexes = GetCellIndexes(cell);
        if (cellIndexes.x == -1 || cellIndexes.y == -1) {
            console.error("Failed to find cell indexes");
            return;
        }

        SendMessage({
            type: GameStateUpdateType.itemClaimed,
            data: {
                x: cellIndexes.x,
                y: cellIndexes.y,
                claimedBy: playerTeam.value
            }
        });
    }

    function GetCellIndexes(cell: GameStateItem) {
        for (let i = 0; i < gameState.value.items.length; i++) {
            for (let j = 0; j < gameState.value.items[i].length; j++) {
                if (gameState.value.items[i][j] == cell) {
                    return { x: i, y: j };
                }
            }
        }
        return { x: -1, y: -1 };
    }
</script>

<template>
    <!--A popup that shows whenever a cell is selected, to show more detailed information and allow the user to claim it.-->
    <div class="selected_cell_popover" v-if="selectedCell">
        <div class="selected_cell_popover_content">
            <div class="selected_cell_popover_content_objective">
                <div class="selected_cell_popover_content_objective_icon">{{ selectedCell.objective.thumbnailEmoji }}</div>
                <div class="selected_cell_popover_content_objective_name">{{ selectedCell.objective.name }}</div>
                <div class="selected_cell_popover_content_objective_description">{{ selectedCell.objective.description }}</div>
            </div>
            <div class="selected_cell_popover_content_claimedby">
                <a v-if="selectedCell.claimedBy != null">Currently claimed by <b v-bind:style="{ color: teams[selectedCell.claimedBy].color}">{{ teams[selectedCell.claimedBy].name }}.</b></a>
                <a v-else>Currently not claimed by anyone.</a>
            </div>
            <div class="selected_cell_popover_content_buttons">
                <button @click="ClaimCell(selectedCell)">Claim</button>
                <button @click="selectedCell = null">Close</button>
            </div>
        </div>
    </div>

    <!--The game board, with each cell represented-->
    <div class="gamecontainer">
        <table class="gameboard">
            <tr class="gameboard_row" v-for="row in gameState.items">
                <td class="gameboard_cell" @click="SelectCell(cell)" v-for="cell in row" v-bind:style="{ backgroundColor: teams[cell.claimedBy || -1]?.color ?? '' }">
                    <div class="gameboard_cell_icon">{{ cell.objective.thumbnailEmoji }}</div>
                    <div class="gameboard_cell_name">{{ cell.objective.name }}</div>
                </td>
            </tr>
        </table>

        <div class="teamscontainer">
            <div class="teams">
                <p class="above_teams_text">You are on team <b v-bind:style="{ color: teams[playerTeam].color }">{{ teams[playerTeam].name }}</b></p>
                <p class="above_teams_text">Click any team below to switch teams.</p>
                <div class="team" v-for="(team, index) in teams.slice(1)" v-bind:style="{ backgroundColor: team.color }" v-on:click="playerTeam = index + 1">
                    <div class="team_name">{{ team.name }}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .gamecontainer {
        display: flex;
        height: 100%;
        width: 100%;
        justify-content: center;
    }

    .gameboard {
        border-radius: 20px;
        max-width: 90vw;
        max-height: 90vh;
        width: 90vh;
        height: 90vw;
        margin: auto;
        border-collapse: collapse;
        table-layout: fixed;
    }

    .gameboard_cell {
        border: 1px solid var(--light);
        text-align: center;
        color: white;
    }

    .gameboard_cell_icon {
        text-align: center;
        font-size: 8vh;
    }

    .gameboard_cell_name {
        text-align: center;
        height: 2.5em;
        font-size: 4vh;
        overflow: hidden;
        color: black;
    }

    @media (orientation: portrait) {
        .gameboard_cell_icon {
            text-align: center;
            font-size: 8vw;
        }

        .gameboard_cell_name {
            text-align: center;
            height: 2.5em;
            font-size: 4vw;
        }   
    }

    .teamscontainer {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 90%;
        width: 10%;
        margin-left: 1%;
    }

    .teams {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 80vh;
        width: 100%;
    }

    .team {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 20%;
        width: 100%;
        border-radius: 20px;
        margin: 5px;
    }

    .team_name {
        font-size: 2vw;
        text-align: center;
    }

    .above_teams_text {
        font-size: 1vw;
        text-align: center;
        color: white;
    }

    .team:hover {
        cursor: pointer;
    }

    .selected_cell_popover {
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100%;
        z-index: 1;
        background-color: rgba(58, 58, 58, 0.918);
    }

    .selected_cell_popover_content {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        border-radius: 20px;
        padding: 20px;
        width: 50%;
        height: 50%;
        background-color: var(--indian);
    }

    .selected_cell_popover_content_objective {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 50%;
        width: 100%;
    }   

    .selected_cell_popover_content_objective_icon {
        font-size: 6vw;
        text-align: center;
    }

    .selected_cell_popover_content_objective_name {
        font-size: 2vw;
        text-align: center;
        color: white;
    }

    .selected_cell_popover_content_objective_description {
        font-size: 1vw;
        text-align: center;
        color: white;
    }

    .selected_cell_popover_content_claimedby {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 20%;
        width: 100%;
    }

    .selected_cell_popover_content_buttons {
        display: flex;
        flex-direction: row;
        justify-content: space-around;
        align-items: center;
        height: 30%;
        width: 100%;
    }

    .selected_cell_popover_content_buttons button {
        font-size: 1vw;
        text-align: center;
        color: white;
        background-color: var(--glaucous);
        border-radius: 10px;
        padding: 10px;
        border: 1px solid var(--light);
    }

</style>