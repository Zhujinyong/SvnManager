import request from '@/utils/request'

export function getRevisionPublishList(params) {
  return request({
    url: '/revision-publish/list',
    method: 'get',
    params
  })
}

export function addRevisionPublish(params) {
  return request({
    url: '/revision-publish/add',
    method: 'post',
    params
  })
}