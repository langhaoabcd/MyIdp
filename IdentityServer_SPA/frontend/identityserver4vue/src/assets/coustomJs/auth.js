import Oidc from 'oidc-client'
var oidcConfig
var mgr
var xhr = new XMLHttpRequest()
xhr.open('get', '/config.json', false)
xhr.send()

oidcConfig = JSON.parse(xhr.responseText).authConfig
oidcConfig.userStore = new Oidc.WebStorageStateStore({
  store: window.localStorage
})
Oidc.Log.logger = console
mgr = new Oidc.UserManager(oidcConfig)
console.log(mgr)
mgr.events.addUserLoaded(function(user) {
  console.log(user)
})
mgr.events.addUserUnloaded(function() {})
mgr.events.addAccessTokenExpired(function() {
  console.log('Access token expiring...' + new Date())
  mgr.removeUser()
  mgr.signinRedirect()
})
mgr.events.addSilentRenewError(function(err) {
  console.log(err)
  mgr.signinRedirect()
})
mgr.events.addUserSignedIn(function() {})
mgr.events.addUserSignedOut(function() {
  console.log('登出')
  // mgr.removeUser()
})
function login() {
  return mgr.signinRedirect()
}
function logout() {
  console.log('登出')
  return mgr.signoutRedirect()
}
function getUser() {
  return mgr.getUser()
}
function callback() {
  return mgr.signinRedirectCallback()
}
function refresh() {
  console.log('刷新')
  return mgr.signinSilentCallback()
}
async function getAuthState() {
  const user = await this.getUser()
  return user != undefined && !user.expired
}
export default {
  login,
  logout,
  getUser,
  callback,
  refresh,
  getAuthState
}
