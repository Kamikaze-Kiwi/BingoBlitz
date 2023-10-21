<script setup lang="ts">
  import { ref } from 'vue'
  import axios from 'axios';

  let items = ref([] as string[]);

  GetItems(0, 10);

  function GetItems(start: number, amount: number) {
    axios.get<string[]>('http://localhost:4002/api/cards/get?start=' + start + '&amount=' + amount)
      .then(response => {
        items.value = response.data;
      })
      .catch(function (error) {
      console.log(error);
    })
  }
</script>

<template>
  <div class="centerContentHorizontal" style="color: var(--light);">
    <h1>This is the community hub page</h1>
    <RouterLink to="/">Return to home</RouterLink>
    <br>
      <a v-for="item in items">
        {{ item }}
      </a>
  </div>
</template>