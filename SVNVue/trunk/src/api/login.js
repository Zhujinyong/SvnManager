import request from '@/utils/request'

export function login(username, password) {
  return request({
    url: '/account/token',
    method: 'post',
    data: {
      userName: username,
      password: password
    }
  })
}

export function getInfo(token) {
  return request({
    url: '/account/infomation',
    method: 'get'
  })
}

export function logout() {
  return request({
    url: '/account/logout',
    method: 'post'
  })
}
