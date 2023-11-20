<script setup lang="ts">
    import { ref } from 'vue'
    import axios from 'axios';
    import type ObjectiveCollection from '@/types/objectiveCollection';
    import type Objective from '@/types/objective';

    let objectiveCollection = ref({} as ObjectiveCollection);
    objectiveCollection.value.objectives = [];

    function AddObjective(){
        objectiveCollection.value.objectives.push({} as Objective);
    }

    function SaveCollection(){

    }
</script>



<template>
    <div class="formmain">
        <label>
            Collection name:
            <input type="text" placeholder="Collection name" />
        </label>

        <br/>
        <br/>

        <div v-for="objective in objectiveCollection.objectives">
            <label>
                Objective name:
                <input type="text" placeholder="A short name..." maxlength="20" />
            </label>
            <br/>
            <br/>
        </div>

        <br/>

        <button v-on:click="AddObjective">Add objective</button>
    </div>
</template>

<style scoped>
    .formmain {
        display: flex;
        flex-direction: column;
        align-items: center;
        color: white;
    }
</style>