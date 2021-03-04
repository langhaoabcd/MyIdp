import Vue from 'vue'
import App from './App.vue'
import { router, farmeOut } from './router'
import store from './store'
import ElementUI from 'element-ui'
import './assets/css/element.scss'
import './assets/css/Common.css'
import { get } from './utils/axios'
import authService from './assets/coustomJs/auth'
Vue.config.productionTip = false
Vue.prototype.$authServices = authService
get('config.json').then(res => {
  Vue.prototype.$config = res
  Vue.prototype.$api = require('./utils/api').default
})
console.log(router)
Vue.use(ElementUI)
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
router.beforeEach(async (to, from, next) => {
  //to.path 目标路由
  //next 必须有否则程序不会继续执行
  console.log(farmeOut)
  console.log(farmeOut.some(x => x.name == to.name))
  if (farmeOut.some(x => x.name == to.name)) {
    store.commit('updateFarmeIn', false)
  } else {
    store.commit('updateFarmeIn', true)
  }
  var res = await authService.getAuthState()
  console.log(store)
  store.commit('updateAuthState', res)
  if (res == false) {
    await authService.login()
  } else {
    next()
  }
})
