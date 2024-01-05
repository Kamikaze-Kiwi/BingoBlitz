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
        { name: 'Red', color: '#ed5045', players: [] as string[]},
        { name: 'Blue', color: '#4550ed', players: [] as string[] },
        { name: 'Green', color: '#3cba4c', players: [] as string[] },
        { name: 'Yellow', color: '#d6e64c', players: [] as string[] },
        { name: 'Orange', color: '#de9835', players: [] as string[] },
        { name: 'Pink', color: '#eb79ed', players: [] as string[] },
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
    //

    const gameId = useRoute().params.id as string;
    let playerTeam = ref(1); 

    const ably = new Ably.Realtime.Promise('QSaqKA.ItBuuQ:qp_nN-UeqeNF4GG4zzrhr-0iSOTkON2EZCZpBqpbCsQ');
    const channel = ably.channels.get('game.' + gameId);

    channel.subscribe('gamestate', (message: any) => {
        HandleIncomingMessage(message.data as GameStateUpdate);
    });

    function SendMessage(message: GameStateUpdate){
        channel.publish('gamestate', message);
    }

    function HandleIncomingMessage(message: GameStateUpdate) {
        switch (message.type) {
            case GameStateUpdateType.itemClaimed:
                gameState.value.items[message.data.x][message.data.y].claimedBy = message.data.claimedBy;
                break;
        }
    }

    function SelectCell(cell: GameStateItem) {
        selectedCell.value = cell;
    }

    function ClaimCell(cell: GameStateItem, claim: boolean = true) {
        cell.claimedBy = claim ? playerTeam.value : null;
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
                claimedBy: cell.claimedBy
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
            <br/>
            <div class="selected_cell_popover_content_claimedby">
                <a v-if="selectedCell.claimedBy != null">Currently claimed by <b v-bind:style="{ color: teams[selectedCell.claimedBy].color}">{{ teams[selectedCell.claimedBy].name }}.</b></a>
                <a v-else>Currently not claimed by anyone.</a>
            </div>
            <br/>
            <div class="selected_cell_popover_content_buttons">
                <button v-if="selectedCell.claimedBy == null" @click="ClaimCell(selectedCell)" v-bind:style="{ backgroundColor: teams[playerTeam].color }">Claim</button>
                <button v-else-if="selectedCell.claimedBy == playerTeam" @click="ClaimCell(selectedCell, false)" v-bind:style="{ backgroundColor: teams[playerTeam].color }">Unclaim</button>
                <button style="background-color: var(--indian);" @click="selectedCell = null">Close</button>
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
            <a class="above_teams_text_1">You are on team <b v-bind:style="{ color: teams[playerTeam].color }">{{ teams[playerTeam].name }}</b></a>
            <a class="above_teams_text_2">Click any team below to switch teams.</a>
            <div class="teams">
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
        overflow: hidden;
        align-items: center;
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
        margin: 1%;
    }

    .gameboard_cell {
        border: 1px solid black;
        text-align: center;
        background-color: var(--light);
    }

    .gameboard_cell:hover {
        cursor: pointer;
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

    .teamscontainer {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 90%;
        margin: 1%;
        flex-direction: column;
        width: 20%;
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
        min-height: 10%;
        width: 100%;
        border-radius: 20px;
        margin: 5px;
    }

    .team_name {
        font-size: 2vw;
        text-align: center;
    }

    .above_teams_text_1 {
        font-size: 3vh;
        text-align: center;
        color: white;
    }

    .above_teams_text_2 {
        font-size: 1.5vh;
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
        height: 100vh;
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
        background-color: var(--eerie);
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
        font-size: 32vh;
        text-align: center;
    }

    .selected_cell_popover_content_objective_name {
        font-size: 8vh;
        text-align: center;
        color: white;
    }

    .selected_cell_popover_content_objective_description {
        font-size: 3vh;
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
        font-size: 3vh;
        color: white;
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
        font-size: 3vh;
        text-align: center;
        color: white;
        border-radius: 10px;
        padding: 10px;
        border: 1px solid var(--light);
    }

    @media (orientation: portrait) {
        .gamecontainer {
            flex-direction: column;
        }

        .gameboard_cell_icon {
            text-align: center;
            font-size: 8vw;
        }

        .gameboard_cell_name {
            text-align: center;
            height: 2.5em;
            font-size: 4vw;
        }   

        .teamscontainer {
            height: 15% !important;
            width: 90% !important;
        }

        .teams {
            flex-direction: row !important;
            height: 100% !important;
        }

        .team {
            height: 80%;
        }

        .selected_cell_popover_content_objective_icon {
            font-size: 32vw;
        }

        .selected_cell_popover_content_objective_name {
            font-size: 8vw;
        }

        .selected_cell_popover_content_objective_description {
            font-size: 3vw;
        }

        .selected_cell_popover_content_buttons button {
            font-size: 3vw;
        }

        .selected_cell_popover_content_claimedby {
            font-size: 3vw;
        }
    }

</style>