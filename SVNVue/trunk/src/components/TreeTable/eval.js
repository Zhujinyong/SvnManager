/**
*/
'use strict'
import Vue from 'vue'
export  function treeToArray(data, expandAll, parent = null, level = null) {
  let tmp = []
  Array.from(data).forEach(function(record) {
    if (record._expanded === undefined) {
      Vue.set(record, '_expanded', expandAll)
    }
    let _level = 1
    if (level !== undefined && level !== null) {
      _level = level + 1
    }
    Vue.set(record, '_level', _level)
    // 如果有父元素
    if (parent) {
      Vue.set(record, 'parent', parent)
    }
    tmp.push(record)
    if (record.children && record.children.length > 0) {
      const children = treeToArray(record.children, expandAll, record, _level)
      tmp = tmp.concat(children)
    }
  })
  return tmp
}

export  function OrderByDesc(arr,func) {
  var m = {};
  for (var i = 0; i < arr.length; i++) {
      for (var k = 0; k < arr.length; k++) {
          if (func(arr[i]) > func(arr[k])) {
              m = arr[k];
              arr[k] = arr[i];
              arr[i] = m;
          }
      }
  }
  return arr;
}
