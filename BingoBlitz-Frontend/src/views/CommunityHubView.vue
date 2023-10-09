<script setup lang="ts">
  import axios from 'axios';

  axios.get('http://localhost:4002/Card?start=0&amount=10')
    .catch(function (error) {
      console.log(error);
    })
    .then(function (response) {
      console.log(response);
    });
</script>

<template>
  <div class="centerContentHorizontal">
    <h1>This is the community hub page</h1>
    <br>
    <!-- <li v-for="item in items" :key="item.id">
      {{ item.text }}
    </li> -->
  </div>
</template>