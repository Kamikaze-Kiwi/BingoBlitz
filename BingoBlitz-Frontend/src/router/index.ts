import { createRouter, createWebHistory } from 'vue-router'
import { authGuard } from "@auth0/auth0-vue";

import HomeView from '../views/HomeView.vue'
import CommunityHubView from '../views/CommunityHubView.vue'
import CreateObjectiveCollectionView from '../views/CreateObjectiveCollectionView.vue'
import ProfileView from '../views/ProfileView.vue'
import NotFoundView from '../views/NotFoundView.vue'
import PlayGameView from '../views/PlayGameView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/communityhub',
      name: 'communityhub',
      component: CommunityHubView
    },
    {
      path: '/CommunityHub/create',
      name: 'createobjectivecollection',
      component: CreateObjectiveCollectionView,
      beforeEnter: authGuard
    },
    {
      path: '/Profile',
      name: 'profile',
      component: ProfileView,
      beforeEnter: authGuard
    },
    {
      path: '/playgame/:id',
      name: 'playgame',
      component: PlayGameView,
    },
    {
      path: '/:catchAll(.*)',
      name: 'notfound',
      component: NotFoundView
    }
  ]
})

export default router
