import axios from 'axios' // 引入axios // 引入qs模块，用来序列化post类型的数据，后面会提到
import Vue from 'vue'
import Qs from 'qs'
/**
 * 自定义实例默认值
 */
var instance = axios.create({
  baseURL: '',
  // 公	共接口url（如果有多个的公共接口的话，需要处理）
  timeout: 30000, // 请求超时
  withCredentials: true
})
instance.defaults.headers.post['Content-Type'] =
  'application/json; charset=UTF-8'

// /api/getUserById

// 请求拦截器, 进行一个全局loading  加载，这种情况下所有的接口请求前 都会加载一个loading

/**
 * 添加请求拦截器 ，意思就是发起请求接口之前做什么事，一般都会发起加载一个loading
 * */

//  如果不想每个接口都加载loading ，就注释掉请求前拦截器,在http这个类中处理

instance.interceptors.request.use(
  config => {
    return config
  },
  error => {
    console.warn(error)
    return Promise.reject(error)
  }
)

/**
 * 添加响应拦截器，意思就是发起接口请求之后做什么事，此时只有两种情况，
 * 要么成功，要么失败，但是不管成功，还是失败，我们都需要关闭请求之前的
 * 发起的loading，那既然要处理loading，就把loading做成全局的了，
 * 这里自定义一个处理加载loding 和关闭loading的方法，而且这个loading
 * 要不要加载，会根据外部传入的布尔值来决定，默认是false:不展示
 * */

instance.interceptors.response.use(
  function(response) {
    if (response.config.method !== 'get') {
      Vue.prototype.$message.success('操作成功！')
    }
    return response
  },
  function(err) {
    console.log(err)
    if (err && err.response) {
      switch (err.response.status) {
        case 400:
          err.message = err.response.data.msg
          if (err.response.data.data) {
            err.message += ':'
            for (const key in err.response.data.data) {
              if (key == err.response.data.data.length - 1) {
                err.message += err.response.data.data[key]
              } else {
                err.message += err.response.data.data[key] + ','
              }
            }
          }
          break

        case 401:
          err.message = '未授权，请登录'
          break

        case 403:
          err.message = '跨域拒绝访问'
          break

        case 404:
          err.message = `请求地址出错: ${err.response.config.url}`
          break

        case 408:
          err.message = '请求超时'
          break

        case 500:
          err.message = '服务器内部错误'
          break

        case 501:
          err.message = '服务未实现'
          break

        case 502:
          err.message = '网关错误'
          break

        case 503:
          err.message = '服务不可用'
          break

        case 504:
          err.message = '网关超时'
          break

        case 505:
          err.message = 'HTTP版本不受支持'
          break

        default:
      }
      Vue.prototype.$message.error(err.message)
    }

    return Promise.reject(err)
  }
)
/**
 * get方法，对应get请求
 * @param {String} url [请求的url地址]
 * @param {Object} params [请求时携带的参数]
 */
export function get(url, params) {
  return new Promise((resolve, reject) => {
    instance
      .get(url, {
        params: params
      })
      .then(res => {
        resolve(res.data)
      })
      .catch(err => {
        reject(err.data)
      })
  })
}
/**
 * post方法，对应post请求
 * @param {String} url [请求的url地址]
 * @param {Object} params [请求时携带的参数]
 */
export function post(url, params) {
  return new Promise((resolve, reject) => {
    instance
      .post(url, JSON.stringify(params))
      .then(res => {
        resolve(res.data)
      })
      .catch(err => {
        reject(err.data)
      })
  })
}
export function deleteR(url, params) {
  return new Promise((resolve, reject) => {
    instance
      .delete(url, { params: params })
      .then(res => {
        resolve(res.data)
      })
      .catch(err => {
        reject(err.data)
      })
  })
}
export function put(url, params) {
  return new Promise((resolve, reject) => {
    instance
      .put(url, params)
      .then(res => {
        resolve(res.data)
      })
      .catch(err => {
        reject(err.data)
      })
  })
}
export function postFormData(url, params) {
  return new Promise((resolve, reject) => {
    instance
      .post(url, params, {
        headers: {
          'Content-Type':
            'multipart/form-data;boundary = ' + new Date().getTime()
        }
      })
      .then(res => {
        resolve(res.data)
      })
      .catch(err => {
        reject(err.data)
      })
  })
}
