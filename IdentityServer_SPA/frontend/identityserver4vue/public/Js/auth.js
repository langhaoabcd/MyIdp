import Oidc from 'oidc-client'
import Vue from 'vue'
import { mapMutations } from 'vuex'
debugger
let config = Vue.prototype.$config
let oidcConfig = config.authConfig
let mgr = new Oidc.UserManager(oidcConfig)
mgr.events.addUserLoaded(function(user) {})
mgr.events.addUserUnloaded(function() {})
mgr.events.addAccessTokenExpiring(function() {
  console.log('Access token expiring...' + new Date())
  mgr.removeUser()
  mgr.signinRedirect()
})
mgr.events.addSilentRenewError(function(err) {
  console.log(err)
  mgr.signinRedirect()
})
mgr.events.addUserSignedOut(function() {
  mgr.removeUser()
})
async function login() {
  await mgr.signinRedirect()
}
async function logout() {
  await mgr.signoutRedirect()
}
async function getUser() {
  await mgr.getUser()
}
async function callback() {
  await mgr.signinRedirectCallback()
}
async function refresh() {
  await mgr.signinSilentCallback()
}
async function updateAuthState() {
  const user = await getUser()
  if (user != null && !user.expired) {
    mapMutations.updateAuthState(false)
    return true
  }
  return false
}
export default {
  login,
  logout,
  getUser,
  callback,
  refresh,
  updateAuthState
}
