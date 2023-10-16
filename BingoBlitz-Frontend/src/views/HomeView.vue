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
          <button class="alignBottom" type="button" style="margin-top: auto;">
            <router-link to="/creategame" tag="button">Create</router-link>
          </button>
      </div>

      <!--Join a game-->
      <div class="box">
          <a class="boxheader">Join a game</a>
          <a>Join an existing game with a 6-character code!</a>
          <hr/>
          <input type="text" placeholder="Enter code" maxlength="6">
          <button class="alignBottom" type="button" style="margin-top: auto;">Join</button>
      </div>

      <!--Browse the community hub-->
      <div class="box">
          <a class="boxheader">Use the community hub</a>
          <a>Use custom packs shared by the community, or create and share your own!</a>
          <hr/>
          <div style="display: flex; justify-content: space-evenly; margin-top: auto;">
            <button class="alignBottom" type="button" style="min-width: 40%;">
              <router-link to="/CommunityHub" tag="button">Browse</router-link>
            </button>
            <button class="alignBottom" type="button" style="min-width: 40%;">
              <router-link to="/CommunityHub/create" tag="button">Create</router-link>
            </button>
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
  height: 100%;
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