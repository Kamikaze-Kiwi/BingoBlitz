<script setup lang="ts">
  import { ref } from 'vue'
  import axios from 'axios';
  import type IterableObjectiveCollectionData from '@/types/iterableObjectiveCollectionData';
  import type ObjectiveCollection from '@/types/objectiveCollectionData';
  const communityHubEndpoint = import.meta.env.VITE_COMMUNITYHUB_API as string;

  let searchFilter = ref('');
  let filter = ref('');
  let items = ref([] as ObjectiveCollection[]);
  let continuationToken : string | null = null;
  let hasMoreItems = ref(false);
  let hasLoaded = ref(false);
  let detailedItem = ref(null as ObjectiveCollection|null);

  GetItems();

  function UpdateFilter() {
    filter.value = searchFilter.value;
    continuationToken = null;

    GetItems();
  }

  function ShowDetails(item: ObjectiveCollection) {
    GetItemDetails(item);
    detailedItem.value = item;
  }

  function GetItems() {
    axios.get<IterableObjectiveCollectionData>(communityHubEndpoint + 'objectives/collections/getqueryable', {
      params: {
        continuationToken: continuationToken,
        filter: filter.value,
        count: 5
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
      })
      .catch(function (error) {
      console.log(error);
    }).finally(() => {
      hasLoaded.value = true;
    });
  }

  function GetItemDetails(item: ObjectiveCollection) {
    if (item.objectives != null && item.objectives.length > 0) {
      return;
    }

    axios.get<ObjectiveCollection>(communityHubEndpoint + 'objectives/collections/getbyid?id=' + item.id)
      .then(response => {
        item.objectives = response.data.objectives;
      })
      .catch(function (error) {
      console.log(error);
    });
  }
</script>

<template>
  <div class="centerContentHorizontal" style="color: var(--light);">
    <h1>This is the community hub page</h1>
    <RouterLink to="/">Return to home</RouterLink>
    <br>

    <div>
      <input class="noBorderRadiusRight" type="text" v-model="searchFilter" placeholder="Search for a collection...">
      <button class="noBorderRadiusLeft" v-on:click="UpdateFilter">Go!</button>
    </div>

    <div class="objectiveContainer">
      <div v-for="item in items" class="objectiveItem">
        <a class="objectiveTitle"> {{ item.name }}</a>
        <a class="objectiveSubtitle">By {{ item.userName }}</a>
        <a class="objectiveCount"> {{ item.objectiveCount }} objectives </a>
        <hr>
        <div class="buttonDrawer">
          <button v-if="detailedItem !== item" @click="ShowDetails(item)">View details</button>
          <button v-else @click="detailedItem = null">Hide details</button>
          <button>Create lobby</button>
        </div>
        <div v-if="detailedItem === item" class="itemDetails">
          <ul>
            <li v-for="objective in item.objectives">
              {{ objective.thumbnailEmoji + '|' + objective.name }}
            </li>
          </ul>
        </div>
      </div>
    </div>
    <h3 v-if="!hasLoaded">Loading items...</h3>
    <button v-else-if="hasMoreItems" @click="GetItems">Load more</button>
    <h3 v-else>Reached bottom of results...</h3>
  </div>
</template>


<style scoped>
.objectiveContainer{
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
  max-width: 40ch;
}

.objectiveItem{
  background-color: var(--ecru);
  color: black;
  border-radius: 5px;
  padding: 10px;
  margin: 10px;
  width: 100%;
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