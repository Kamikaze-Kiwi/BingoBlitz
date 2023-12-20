<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
const { isLoading, user, isAuthenticated } = useAuth0();

import { ref } from 'vue'
import axios from 'axios';
import type IterableObjectiveCollectionData from '@/types/iterableObjectiveCollectionData';
import type ObjectiveCollection from '@/types/objectiveCollectionData';
const communityHubEndpoint = import.meta.env.VITE_COMMUNITYHUB_API as string;

let items = ref([] as ObjectiveCollection[]);
let searchFilter = ref('');
let filter = ref('');
let continuationToken: string | null = null;
let hasMoreItems = ref(false);
let hasAnyItems = ref(false);

GetItems();

function GetItems() {
    axios.get<IterableObjectiveCollectionData>(communityHubEndpoint + 'objectives/collections/getqueryablebyuser', {
        params: {
            continuationToken: continuationToken,
            filter: filter.value,
            count: 10,
            userId: user.value?.sub?.split('|')[1]
        }
    })
        .then(response => {
            if (continuationToken == null) {
                items.value = response.data.objectiveCollections;
            }
            else {
                items.value = items.value.concat(response.data.objectiveCollections);
            }

            continuationToken = response.data.continuationToken;

            hasMoreItems.value = (continuationToken != null);

            if (!hasAnyItems.value) hasAnyItems.value = (items.value.length > 0);
        })
        .catch(function (error) {
            console.log(error);
        });
}

function UpdateFilter() {
    filter.value = searchFilter.value;
    continuationToken = null;

    GetItems();
}

</script>


<template>
    <div class="container">
        <!--Previous games-->
        <div class="box">
            <div class="header">
                <a class="title">Previous Games</a>
            </div>

            <div class="contentList">
                Once you've played some games, they will appear here!
            </div>
        </div>

        <!--Objective collections:-->
        <div class="box">
            <div class="header">
                <a class="title">Created collections</a>
            </div>

            <div class="contentList">
                <div v-if="hasAnyItems">
                    <div>
                        <input class="noBorderRadiusRight" type="text" v-model="searchFilter"
                            placeholder="Search for a collection...">
                        <button class="noBorderRadiusLeft" v-on:click="UpdateFilter">Go!</button>
                    </div>
                    <div v-for="item in items" class="objectiveItem">
                        <a class="objectiveTitle"> {{ item.name }}</a>
                        <a class="objectiveCount"> {{ item.objectiveCount }} objectives </a>
                        <hr>
                        <div class="buttonDrawer">
                            <button>View details</button>
                            <button>Create lobby</button>
                            <button>Delete</button>
                        </div>
                    </div>
                </div>
                <div v-else>
                    Once you've created some collections, they will appear here!
                </div>
            </div>
        </div>
    </div>
</template>


<style scoped>
.container {
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    text-align: center;
    margin-top: 2cm;
    color: var(--light);
    max-height: 80%;
}

.box {
    width: 45%;
    max-height: 90%;
}

.header {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    background-color: var(--glaucous);
    border-radius: 10px 10px 0 0;
}

.contentList {
    background-color: var(--eerie);
    border-radius: 0 0 10px 10px;
    padding: 1rem;
    border: 2px solid var(--glaucous);
    max-height: 90%;
    overflow: auto;
}

.title {
    font-size: 2rem;
    font-weight: bold;
    color: black;
    text-align: center;
    width: 100%;
}

.objectiveItem {
    background-color: var(--ecru);
    color: black;
    border-radius: 5px;
    padding: 10px;
    margin: 10px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    align-items: center;
}

.objectiveTitle {
    font-size: 1.5em;
    font-weight: bold;
    text-align: center;
    word-break: break-all;
}

.objectiveCount {
    font-size: 1.2em;
}

.buttonDrawer {
    display: flex;
    flex-direction: row;
    justify-content: space-evenly;
    align-items: center;
    width: 100%;
}
</style>