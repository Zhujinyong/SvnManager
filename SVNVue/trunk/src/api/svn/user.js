import request from '@/utils/request'

export function getSvnUserList(params) {
  return request({
    url: '/svnuser/list',
    method: 'get',
    params
  })
}

export function addSvnUser(params) {
  return request({
    url: '/svnuser/add',
    method: 'post',
    params
  })
}

export function updateSvnUser(params) {
  return request({
    url: '/svnuser/update',
    method: 'post',
    params
  })
}