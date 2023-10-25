import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import CommunityHubView from '../views/CommunityHubView.vue'
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
      path: '/:catchAll(.*)',
      name: 'notfound',
      component: NotFoundView
    }
  ]
})

export default router
