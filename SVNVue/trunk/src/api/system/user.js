import request from '@/utils/request'

export function getSystemUserList(params) {
  return request({
    url: '/systemuser/list',
    method: 'get',
    params
  })
}

export function addSystemUser(params) {
  return request({
    url: '/systemuser/add',
    method: 'post',
    params
  })
}

export function updateSystemUser(params) {
  return request({
    url: '/systemuser/update',
    method: 'post',
    params
  })
}