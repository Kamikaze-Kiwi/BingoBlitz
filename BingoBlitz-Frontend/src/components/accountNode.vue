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
            <span>Signed in as <RouterLink to="profile">{{ user.name }}</RouterLink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <a href="" @click="Logout">Logout</a>
        </div>
        <div v-else>
            <a href="" @click="Login">Login</a>
        </div>
    </div>
</template>


<style scoped>

</style>