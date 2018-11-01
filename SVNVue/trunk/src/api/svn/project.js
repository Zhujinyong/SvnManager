import request from '@/utils/request'

export function getProjectList(params) {
  return request({
    url: '/project/list',
    method: 'get',
    params
  })
}

export function addProject(params) {
  return request({
    url: '/project/add',
    method: 'post',
    params
  })
}

export function deleteProject(params) {
  return request({
    url: '/project/delete',
    method: 'post',
    params
  })
}

export function updateProject(params) {
  return request({
    url: '/project/update',
    method: 'post',
    params
  })
}