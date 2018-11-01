import request from '@/utils/request'

export function getSvnJenkinsList(params) {
  return request({
    url: '/svn-jenkins/list',
    method: 'get',
    params
  })
}

export function addSvnJenkins(params) {
  return request({
    url: '/svn-jenkins/add',
    method: 'post',
    params
  })
}

export function UpdateSvnJenkins(params) {
  return request({
    url: '/svn-jenkins/update',
    method: 'post',
    params
  })
}