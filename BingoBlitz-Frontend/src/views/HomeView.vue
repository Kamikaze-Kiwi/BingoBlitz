<script setup lang="ts">
  import axios from 'axios';

  function JoinGame(){
    let code: string = (document.getElementById("JoinGameIdInput") as HTMLInputElement).value;
    axios.post<string>('http://localhost:4001/api/game/join?gameId=' + code)
      .then(response => {
        alert(response.data);
      })
      .catch(function (error) {
        console.log(error);
    })
  }
</script>

<template>
  <div class="container">
    <a class="title">Bingo Blitz</a>
    <br/>
    <a class="subtext">What do you want to do?</a>
    <br/>
    <br/>

    <div class="split">

      <!--Create a game-->
      <div class="box">
          <a class="boxheader">Create a game</a>
          <a>Host a new game and invite your friends!</a>
          <hr>
          <router-link to="/creategame" class="alignBottom" style="min-width: 40%; margin-top: auto;" tag="button">Create</router-link>
      </div>

      <!--Join a game-->
      <form class="box">
          <a class="boxheader">Join a game</a>
          <a>Join an existing game with a 6-character code!</a>
          <hr/>
          <input id="JoinGameIdInput" type="text" placeholder="Enter code" maxlength="6">
          <button @click="JoinGame" class="alignBottom" type="button" style="margin-top: auto;">Join</button>
      </form>

      <!--Browse the community hub-->
      <div class="box">
          <a class="boxheader">Use the community hub</a>
          <a>Use custom packs shared by the community, or create and share your own!</a>
          <hr/>
          <div style="display: flex; justify-content: space-evenly; margin-top: auto;">
            <router-link to="/CommunityHub" class="alignBottom" style="min-width: 40%;" tag="button">Browse</router-link>
            <router-link to="/CommunityHub/create" class="alignBottom" style="min-width: 40%;" tag="button">Create</router-link>
          </div>
      </div>

    </div>
  </div>

</template>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 90%;
  text-align: center;
}

.split {
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  align-items: center;
  width: 100%;
}

.box {
  height: 100%;
  width: 25%;
  background-color: var(--ecru);
  color: black;
  border-radius: 10px;
  text-align: center;
  display: flex;
  flex-direction: column;
  padding: 10px;
}

.title {
  font-size: 50px;
  font-weight: bold;
  color: var(--indian);
}

.subtext {
  font-size: 30px;
  color: var(--glaucous);
}

.boxheader {
  font-size: 20px;
  font-weight: bold;
  width: 100%;
  display: block;
}

/* Edits for portrait mode: */
@media (orientation: portrait) {
  .split {
    flex-direction: column !important;
  }
  
  .container {
    justify-content: normal !important;
  }

  .box {
    width: 80% !important;
    margin-bottom: 3vh !important;
  }

  .alignBottom {
    margin-top: 2vh !important;
  }
}
</style>