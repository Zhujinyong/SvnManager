import request from '@/utils/request'
import qs from 'qs'

export function getStatisticsProjectList(params) {
  return request({
    url: '/statistics/treeList',
    method: 'get',
    params,
    paramsSerializer: params => {
      return qs.stringify(params, { indices: false })
    }
  })
}

export function getProjectSvnUserList(params) {
  return request({
    url: '/statistics/submitUser',
    method: 'get',
    params
  })
}

export function getStatisticsSvnUserList(params) {
  return request({
    url: '/statistics/user',
    method: 'get',
    params
  })
}

