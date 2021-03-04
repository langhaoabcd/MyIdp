import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.prototype.$author = 'bob'
Vue.use(VueRouter)
const farmeOut = [
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/Auth/login.vue')
  },
  {
    path: '/callback',
    name: 'callback',
    component: () => import('../views/Auth/callback.vue')
  },
  {
    path: '/logout',
    name: 'logout',
    component: () => import('../views/Auth/logout.vue')
  },
  {
    path: '/error',
    name: 'error',
    component: () => import('../views/Auth/error.vue')
  },
  {
    path: '/refresh',
    name: 'refresh',
    component: () => import('../views/Auth/refresh.vue')
  }
]
const farmeIn = [
  {
    path: '/',
    name: 'main',
    component: () => import('../views/Main.vue')
  },
  {
    path: '/sysuser',
    name: 'sysuser',
    component: () => import('../views/SysUser.vue')
  },
  {
    path: '/clamins',
    name: 'clamins',
    component: () => import('../views/Clamins.vue')
  }
]

const router = new VueRouter({
  mode: 'hash',
  routes: [...farmeOut, ...farmeIn]
})

export { router, farmeOut }
