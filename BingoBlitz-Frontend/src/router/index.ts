import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import CommunityHubView from '../views/CommunityHubView.vue'
import CreateObjectiveCollectionView from '../views/CreateObjectiveCollectionView.vue'
import ProfileView from '../views/ProfileView.vue'
import NotFoundView from '../views/NotFoundView.vue'

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
      component: CreateObjectiveCollectionView
    },
    {
      path: '/Profile',
      name: 'profile',
      component: ProfileView
    },
    {
      path: '/:catchAll(.*)',
      name: 'notfound',
      component: NotFoundView
    }
  ]
})

export default router
