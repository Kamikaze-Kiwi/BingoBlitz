import './assets/main.css';

import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

// import { createAuth0 } from '@auth0/auth0-vue';

const app = createApp(App);

app.use(router);

// app.use(
//     createAuth0({
//         domain: "bingoblitz.eu.auth0.com",
//         clientId: "vUYV4sLn2T4GazU9jgifkYPH9cszJkf0",
//         authorizationParams: {
//             redirect_uri: window.location.origin,
//             audience: "CommunityHub",
//         }
//     })
// );

app.mount('#app');
