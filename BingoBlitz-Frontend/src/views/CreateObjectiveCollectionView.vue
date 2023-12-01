<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios';
import type ObjectiveCollection from '@/types/objectiveCollection';
import type Objective from '@/types/objective';
import EmojiPicker from 'vue3-emoji-picker'
import 'vue3-emoji-picker/css'

import { useAuth0 } from '@auth0/auth0-vue';
const { user } = useAuth0();

let objectiveCollection = ref({} as ObjectiveCollection);
objectiveCollection.value.name = "";
objectiveCollection.value.objectives = [];
AddObjective();


function ToggleObjective(event: Event) {
    let button: HTMLElement = event.target as HTMLElement;

    button.parentElement?.parentElement?.parentElement?.querySelector(".objectiveToggleable")?.classList.toggle("hidden")
        ?? console.error("Failed to toggle objective");
}

function ShowIconPicker(event: Event) {
    document.getElementById("blocker")?.classList.add("blockspage");

    let button: HTMLElement = event.target as HTMLElement;

    button.parentElement?.querySelector(".iconpicker")?.classList.toggle("hidden")
        ?? console.error("Failed to toggle icon picker");
}

function OnSelectIcon(emoji: any, objective: Objective) {
    objective.thumbnailEmoji = emoji.i;
    HideIconPicker();
}

function HideIconPicker() {
    document.getElementById("blocker")?.classList.remove("blockspage");

    Array.from(document.getElementsByClassName("iconpicker")).forEach((element: Element) => {
        element.classList.add("hidden");
    });
}

function AddObjective() {
    objectiveCollection.value.objectives.push({ name: "", description: "" } as Objective);
}

function RemoveObjective(index: number) {
    if (objectiveCollection.value.objectives.length == 1) {
        alert("You can't remove the last objective!");
        return;
    }

    objectiveCollection.value.objectives.splice(index, 1);
}

function SaveCollection() {
    if (objectiveCollection.value.name == "") {
        alert("You must give your collection a name!");
        return;
    }

    if (objectiveCollection.value.objectives.length < 4) {
        alert("You must have at least 4 objectives!");
        return;
    }

    if (objectiveCollection.value.objectives.some(objective => objective.name == "")) {
        alert("You must give all objectives a name!");
        return;
    }

    if (objectiveCollection.value.objectives.some(objective => objective.description == "")) {
        alert("You must give all objectives a description!");
        return;
    }

    if (objectiveCollection.value.objectives.some(objective => objective.thumbnailEmoji == undefined)) {
        alert("You must give all objectives an icon!");
        return;
    }

    objectiveCollection.value.userid = user.value?.sub?.split('|')[1] ?? "0";
    objectiveCollection.value.username = user.value?.name ?? "0";

    axios.post(
        "http://localhost:4002/api/objectives/collections/save",
        objectiveCollection.value,
        {
            headers: {
                'Content-Type': 'application/json'
            },
        }
    )
        .then(response => {
            console.log(response);
            alert("Collection saved and published!");
        })
        .catch(error => {
            console.error(error);
            alert("Failed to save collection. Please try again later.");
        });
}
</script>



<template>
    <!--Blocks any inputs in case a popup is showing, and closes the popup if the user presses outside the popup.-->
    <div id="blocker" v-on:click="HideIconPicker"></div>

    <div class="centerContentHorizontal" style="color: var(--light);">
        <h1>This is the objective collection creation page</h1>
        <RouterLink to="/">Return to home</RouterLink>
        <br />
        <div class="formWrapper centerContentHorizontal">
            <label class="inputLabel">
                Collection name:
                <input type="text" placeholder="Collection name" v-model="objectiveCollection.name" />
            </label>

            <br />
            <br />

            <div class="objectiveCollectionWrapper">
                <div class="objectiveWrapper" v-for="objective in objectiveCollection.objectives">
                    <div class="objectiveHeader">
                        <b style="height: 100%;">{{ objectiveCollection.objectives.indexOf(objective) + 1 }}) {{
                            objective.name }}</b>
                        <div class="buttonDrawer">
                            <button style="margin-right: 10px;" v-on:click="ToggleObjective">Toggle</button>
                            <button v-on:click="RemoveObjective(objectiveCollection.objectives.indexOf(objective))">Remove
                                objective</button>
                        </div>
                    </div>

                    <div class="objectiveToggleable">
                        <hr>
                        <div class="objectiveContent">
                            <div>
                                <label class="inputLabel">
                                    Objective name:
                                    <input type="text" placeholder="A short name..." maxlength="20"
                                        v-model="objective.name" />
                                </label>

                                <label class="inputLabel">
                                    Objective icon:
                                    <button v-on:click="ShowIconPicker" style="background-color: var(--light);">{{
                                        objective.thumbnailEmoji ?? 'Click to pick icon.' }}</button>
                                    <EmojiPicker @select="OnSelectIcon($event, objective)" class="iconpicker hidden"
                                        :native="true" :disable-skin-tones="true" :theme="'light'" :mode="'prepend'"
                                        :disabled-groups="['flags']" />
                                </label>
                            </div>
                            <div style="display: flex;">
                                <label class="inputLabel">
                                    Objective description:
                                    <textarea v-model="objective.description"></textarea>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <br />

            <button v-on:click="AddObjective">Add objective</button>
        </div>

        <button v-on:click="SaveCollection">Save collection</button>
    </div>
</template>

<style scoped>
.formWrapper {
    margin: 20px;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    color: white;
}

.objectiveWrapper {
    margin: 20px;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    color: black;
    background-color: var(--ecru);
}

.objectiveHeader {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}

.buttonDrawer {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}

.objectiveToggleable {
    overflow: hidden;
    transition: max-height 0.2s ease-in-out;
    max-height: 164px;
}

.hidden {
    max-height: 0px;
}

.objectiveContent {
    display: flex;
    flex-direction: row;
}

.inputLabel {
    display: flex;
    flex-direction: column;
    margin: 5px;
    font-size: large;
}

.iconpicker {
    position: absolute;
    z-index: 2;
}

.iconpicker.hidden {
    display: none;
}

textarea {
    resize: none;
    height: 100%;
}

.blockspage {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100%;
    z-index: 1;
}
</style>