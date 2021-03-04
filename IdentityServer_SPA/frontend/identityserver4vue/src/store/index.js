import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    isLogin: false,
    farmeIn: false
  },
  mutations: {
    updateAuthState(state, res) {
      console.log('updateAuthState:' + res)
      if (state.isLogin != res) {
        state.isLogin = res
      }
    },
    updateFarmeIn(state, res) {
      console.log('updateFarmeIn:' + res)
      state.farmeIn = res
    }
  },
  actions: {},
  modules: {}
})
