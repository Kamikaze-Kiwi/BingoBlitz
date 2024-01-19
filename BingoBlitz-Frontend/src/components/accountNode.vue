<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';

const { isLoading, loginWithRedirect, logout, user, isAuthenticated } = useAuth0();


function Login() {
    loginWithRedirect();
}

function Logout() {
    logout();
}
</script>


<template>
    <div>
        <div v-if="isLoading">...</div>
        <div v-else-if="isAuthenticated && user">
            <div class="dropdown">
                <RouterLink :to="{name: 'profile'}" class="dropbtn">Signed in as {{ user.name }}</RouterLink>
                <div class="dropdown-content">
                    <RouterLink :to="{ name: 'profile' }">Profile</RouterLink>
                    <RouterLink :to="{ name: 'profilesettings' }">Settings</RouterLink>
                    <a href="" @click="Logout">Logout</a>
                </div>
            </div>
        </div>
        <div v-else>
            <a href="" @click="Login">Login</a>
        </div>
    </div>
</template>


<style scoped>
/* Dropdown container */
.dropdown {
    position: relative;
    display: inline-block;
}

/* Dropdown button style */
.dropbtn {
    padding: 10px;
    font-size: 16px;
    border: none;
    cursor: pointer;
}

/* Dropdown content (hidden by default) */
.dropdown-content {
    display: none;
    position: absolute;
    top: 100%;
    right: 0;
    background-color: var(--powder);
    min-width: 160px;
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
    z-index: 1;
}

/* Links inside the dropdown */
.dropdown-content a {
    color: black;
    padding: 12px 16px;
    text-decoration: none;
    display: block;
}

/* Change color on hover */
.dropdown-content a:hover {
    background-color: var(--glaucous);
}

/* Show the dropdown when the button is hovered over */
.dropdown:hover .dropdown-content {
    display: block;
}
</style>