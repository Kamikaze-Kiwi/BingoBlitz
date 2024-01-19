<script setup lang="ts">
import { useAuth0, type GetTokenWithPopupOptions } from '@auth0/auth0-vue';
import axios from 'axios';
const { isLoading, user, getAccessTokenWithPopup, isAuthenticated } = useAuth0();

function ShowDeleteProfilePopup() {
    document.getElementsByClassName("DeleteProfilePopup")[0].classList.toggle("show");
}

async function DeleteProfile() {
    let deletePacks = (document.getElementById("deletePacks") as HTMLInputElement).checked;
    let deleteHistoricalGames = (document.getElementById("deleteHistoricalGames") as HTMLInputElement).checked;

    let options: GetTokenWithPopupOptions = {
        authorizationParams: {
            prompt: 'consent',
            audience: 'https://bingoblitz.eu.auth0.com/api/v2/',
            scope: 'delete:current_user'
        }
    };

    let token = await getAccessTokenWithPopup(options);
    if (!user.value || !isAuthenticated) return;

    let response = await axios.delete(
        `https://bingoblitz.eu.auth0.com/api/v2/users/${user.value.sub}`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            },
        }
    );
}
</script>


<template>
    <br/>
    <br/>
    <div class="centerContentHorizontal" v-if="!isLoading">
        <button @click="ShowDeleteProfilePopup">Delete profile</button>
    </div>

    <div class="DeleteProfilePopup">
        <b class="warning">Warning: This action is irreversible!</b>
        <br/>
        <br/>

        <label>Delete all your created packs?
            <input type="checkbox" name="delete" id="deletePacks">
        </label>
        <details>
            <summary>More information:</summary>
            This will also delete all your created packs and all the cards in them.
            <br>
            By not selecting this, all your created packs will continue to exist, but the username will be changed to "Deleted User".
            <br>
            If you do select this, all your created packs will be permanently deleted.
        </details>
        <br/>

        <label>Remove name from historical games?
            <input type="checkbox" name="delete" id="deleteHistoricalGames">
        </label>
        <details>
            <summary>More information:</summary>
            This will replace your name with "Deleted User" in all historical games you have played.
            <br>
            By not selecting this, your name will continue to exist in all historical games you have played. The players you have played with will still see your name in their history.
            <br>
            If you do select this, all your historical games will not contain your username anymore.
        </details>
        <br/>
        <button style="margin-right: 5rem;" @click="DeleteProfile">Delete profile</button>
        <button style="background-color: rgb(110, 238, 110);" @click="ShowDeleteProfilePopup">Cancel</button>
    </div>
</template>


<style scoped>
.DeleteProfilePopup {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    border-radius: 20px;
    padding: 20px;
    background-color: var(--powder);
    display: none;
    width: 60%;
}

.DeleteProfilePopup.show {
    display: block;
}

.warning {
    color: red;
}
</style>