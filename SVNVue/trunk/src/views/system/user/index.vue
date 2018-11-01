<template>
  <div class="app-container">
     <div class="filter-container">
     <el-input  style="width: 200px;" v-model="listQuery.userName" placeholder="请输入域账号" clearable  @change="resetPageIndex" ></el-input>
     <el-input style="width: 200px;"  v-model="listQuery.realName" placeholder="请输入姓名" clearable  @change="resetPageIndex" ></el-input>
     <el-button type="primary" icon="el-icon-search" @click="search">查询</el-button>
     <el-button type="primary" icon="el-icon-plus"   @click="add">添加</el-button>
    </div>
    <el-table
      v-loading="listLoading"
      :data="list"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row style="margin-top:20px;width: 100%" >
      <el-table-column align="center" label="序号" width="60">
        <template slot-scope="scope">
          {{ scope.$index+1 }}
        </template>
      </el-table-column>
      <el-table-column label="账号"  align="center" width="200">
        <template slot-scope="scope">
          {{ scope.row.userName }}
        </template>
      </el-table-column>
      <el-table-column label="姓名" width="110" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.realName}}</span>
        </template>
      </el-table-column>
       <el-table-column label="是否启用" width="110" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.isUse=="1"?"是":"否"}}</span>
        </template>
      </el-table-column>
       <el-table-column label="头像" width="110" align="center">
        <template slot-scope="scope">
          <img :src=" scope.row.picture"  height="40" width="40">
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center">
        <template slot-scope="scope">
          {{ scope.row.description }}
        </template>
      </el-table-column>
       <el-table-column
      align="center"
      label="操作"
      width="250">
      <template slot-scope="scope">
        <el-button @click="openFileDialog(scope.row)"  size="small"  type="primary" icon="el-icon-edit" >编辑</el-button>
      </template>
    </el-table-column>
    </el-table>
    <div class="pagination-container">
      <el-pagination :current-page="listQuery.pageIndex" :page-sizes="[10,20,30,50]" :page-size="listQuery.pageSize" :total="total" layout="total, sizes, prev, pager, next, jumper" @size-change="handleSizeChange" @current-change="handleCurrentChange">
      </el-pagination>
      
    </el-pagination>
    </el-pagination>
    </div>
    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogVisible" :close-on-click-modal="false">
       <el-form ref="form" :model="temp" label-width="120px">
      <el-form-item label="账号">
      <el-input v-model="temp.userName" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="姓名">
      <el-input v-model="temp.realName" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="是否启用">
      <el-switch v-model="temp.isUse" active-value="1" inactive-value="0"></el-switch>
      </el-form-item>
      <el-form-item label="头像地址">
      <el-input type="textarea" v-model="temp.picture" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="备注">
      <el-input type="textarea" v-model="temp.description" placeholder="请输入"></el-input>
      </el-form-item>
      
    </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="save">保存</el-button>
        <el-button  type="primary" @click="dialogVisible=false">关闭</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script>
import { getSystemUserList,addSystemUser,updateSystemUser } from '@/api/system/user'

export default {
   computed: {
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'gray',
        deleted: 'danger'
      }
      return statusMap[status]
    }
  },
  data() {
    return {
      dialogStatus: '',
      textMap: {
        update: '编辑',
        add: '添加'
      },
      temp:{
        userName:'',
        password:'',
        realName:'',
        description:'',
        isUse:0,
        picture:''
      },
     dialogVisible:false,
      currentPage:1,
      total: null,
      list: null,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 10,
        userName: '',
        realName: ''
      }
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
     resetPageIndex(v){
      this.listQuery.pageIndex=1
    },
     search(){
      this.fetchData()
    },
     add(){
      this.resetTemp()
      this.dialogStatus = 'add'
      this.dialogVisible = true
    },
     save(){
      if(this.dialogStatus == 'add')
      {
         addSystemUser(this.temp).then(response => {
            this.dialogVisible=false
            this.$message({
             message: '添加成功',
             type: 'success'
              });
          })
      }
      else if(this.dialogStatus == 'update')
      {
          updateSystemUser(this.temp).then(response => {
            this.dialogVisible=false
            this.$message({
             message: '修改成功',
             type: 'success'
              });
          })
      }
       this.fetchData()
    },
      resetTemp() {
      this.temp = {
        userName:'',
        password:'',
        realName:'',
        description:'',
        isUse:'0',
        picture:''
      }
    },
     openFileDialog(row) {
       this.temp= Object.assign({}, row)
      this.temp.isUse=row.isUse+''
      this.dialogVisible = true
        this.dialogStatus = 'update'
    },
    fetchData() {
      this.listLoading = true
      getSystemUserList(this.listQuery).then(response => {
        this.list = response.data
        this.total=response.page.total
        this.listLoading = false
      })
    },
    handleSizeChange(val) {
      this.listQuery.pageSize = val
      this.fetchData()
    },
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val
      this.fetchData()
    }
  }
}
</script>
