<script setup lang="ts">
  import { ref } from 'vue'
  import axios from 'axios';
  import type ObjectiveCollection from '@/types/objectiveCollection';

  let items = ref([] as ObjectiveCollection[]);

  GetItems();

  function GetItems() {
    axios.get<ObjectiveCollection[]>('http://localhost:4002/api/objectives/collections/get')
      .then(response => {
        items.value = response.data;
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
      <ul>
        <li v-for="item in items">
          <details>
            <summary> <b> {{ item.name }} </b>: {{ item.objectives.length }} objectives</summary>

            <ul>
              <li v-for="objective in item.objectives">
                {{ objective.name }}
              </li>
            </ul>

            <button>Start game with this collection.</button>
          </details>
        </li>
      </ul>
  </div>
</template>