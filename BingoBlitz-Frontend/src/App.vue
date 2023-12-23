<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
// import accountNode from './components/accountNode.vue';
</script>

<template>
  <header>
  </header>
  <nav>
    <RouterLink to="/">Home</RouterLink>
    <!-- <accountNode /> -->
  </nav>
  <RouterView />
</template>

<style scoped>
nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  background-color: var(--ecru);
}
</style>
