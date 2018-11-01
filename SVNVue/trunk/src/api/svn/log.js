import request from '@/utils/request'

export function getLogList(params) {
  return request({
    url: '/log/list',
    method: 'get',
    params
  })
}

export function getLogFileList(params) {
  return request({
    url: '/log/files',
    method: 'get',
    params
  })
}

export function GetFileChange(params) {
  return request({
    url: '/log/file-change',
    method: 'get',
    params
  })
}

export function GetFileContent(params) {
  return request({
    url: '/log/file-content',
    method: 'get',
    params
  })
}
