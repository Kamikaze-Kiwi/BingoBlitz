<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
const { isLoading, user, isAuthenticated } = useAuth0();


</script>


<template>
    <div class="container">
        <div class="box">
            <div class="header">
                <a class="title">Previous Games</a>
            </div>
            
            <div class="contentList">
                Once you've played a game, it will appear here!
            </div>
        </div>

        <div class="box">
            <div class="header">
                <a class="title">Created collections</a>
            </div>

            <div class="contentList">
                Once you've created a collection, it will appear here!
            </div>
        </div>
    </div>

</template>


<style scoped>
.container {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-around;
  text-align: center;
  margin-top: 2cm;
  color: var(--light);
}

.box {
    width: 45%;
}

.header {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    background-color: var(--ecru);
    border-radius: 10px 10px 0 0;
}

.contentList {
    background-color: var(--eerie);
    border-radius: 0 0 10px 10px;
    padding: 1rem;
    border: 2px solid var(--ecru);
}

.title {
  font-size: 2rem;
  font-weight: bold;
  color: black;
  text-align: center;
  width: 100%;
}
</style>