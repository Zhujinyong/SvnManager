import request from '@/utils/request'

export function getProjectRelationTreeList(params) {
  return request({
    url: '/project-relation/tree',
    method: 'get',
    params
  })
}

export function addProjectRelation(params) {
  return request({
    url: '/project-relation/add',
    method: 'post',
    params
  })
}

export function updateProjectRelation(params) {
  return request({
    url: '/project-relation/update',
    method: 'post',
    params
  })
}

export function deleteProjectRelation(params) {
  return request({
    url: '/project-relation/delete',
    method: 'post',
    params
  })
}

export function getProjectTreeList(params) {
  return request({
    url: '/project-relation/treeList',
    method: 'get',
    params
  })
}