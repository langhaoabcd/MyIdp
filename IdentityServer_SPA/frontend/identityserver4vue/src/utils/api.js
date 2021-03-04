import { get, post, postFormData, deleteR, put } from './axios'
import Vue from 'vue'
var baseUrl = Vue.prototype.$config.serveUrl + '/api'
const api = {
  common: {},
  account: {
    login: params => {
      return post(baseUrl + '/account/login', params)
    },
    loginsms: params => {
      return post(baseUrl + '/account/LoginBySms', params)
    },
    thirdlogin: params => {
      return get(baseUrl + '/account/ExternalLogin', params)
    },
    callback: params => {
      return get(baseUrl + params)
    },
    logout: params => {
      return post(baseUrl + '/account/logout', params)
    },
    getError: params => {
      return get(baseUrl + '/account/GetErrorMsg', params)
    }
  }
}
export default api
