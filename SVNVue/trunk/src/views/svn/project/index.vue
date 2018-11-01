<template>
  <div class="app-container">
     <div class="filter-container">
    <el-input  style="width: 200px;" v-model="listQuery.name" placeholder="项目名称" @change="resetPageIndex" clearable></el-input>
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
     <el-table-column align="center" label="序号" width="60"  type="index"
            :index="typeIndex">
       
      </el-table-column>
      <el-table-column label="项目" align="center">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
       <el-table-column label="是否启用" width="110" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.isUse=="1"?"是":"否"}}</span>
        </template>
      </el-table-column>
      <el-table-column label="类型" width="110" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.type==1?"源代码":"产品文档" }}</span>
        </template>
      </el-table-column>
      <el-table-column class-name="status-col" label="负责人" width="100" align="center">
        <template slot-scope="scope">
          {{ scope.row.head }}
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="城市" width="80">
        <template slot-scope="scope">
          {{ scope.row.city }}
        </template>
      </el-table-column>
      <el-table-column label="创建时间" width="200" align="center">
        <template slot-scope="scope">
          {{ scope.row.createTime }}
        </template>
      </el-table-column>
        <el-table-column align="center" label="操作" width="250">
      <template slot-scope="scope">
        <el-button @click="openFileDialog(scope.row)"  size="small"  type="primary" icon="el-icon-edit" >编辑</el-button>
        <el-button @click="deleteProject(scope.row)"  size="small"  type="primary" icon="el-icon-edit" >删除</el-button>
      
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
      <el-form-item label="项目">
      <el-input v-model="temp.name" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="是否启用">
      <el-switch v-model="temp.isUse" active-value="1" inactive-value="0"></el-switch>
      </el-form-item>
      <el-form-item label="类型">
       <el-select v-model="temp.type" placeholder="请选择">
        <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value">
        </el-option>
       </el-select>
      </el-form-item>
      <el-form-item label="负责人">
      <el-input v-model="temp.head" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="创建人">
      <el-input v-model="temp.creator" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="城市">
      <el-input v-model="temp.city" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="地址">
      <el-input type="textarea"  v-model="temp.url" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="备注">
      <el-input type="description" v-model="temp.description" placeholder="请输入"></el-input>
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
import { getProjectList,addProject,updateProject,deleteProject } from '@/api/svn/project'

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
       options: [{
          value: 1,
          label: '源代码'
        }, {
          value: 2,
          label: '产品文档'
        }],
      dialogStatus: '',
      textMap: {
        update: '编辑',
        add: '添加'
      },
      temp:{
        name:'',
        isUse:'0',
        type:1,
        head:'',
        city:'',
        url:'',
        creator:'',
        description:''
      },
     dialogVisible:false,
      currentPage:1,
      total: null,
      list: null,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 10,
        name: ''
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
      deleteProject(row) {
       this.$confirm('此操作将永久删除'+row.name+', 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
           deleteProject({id:row.id}).then(response => {
              this.fetchData()
            this.$message({
             message: '删除成功',
             type: 'success'
              });
          })
        }).catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })     
        })

    },
    typeIndex(index) {
        return index + (this.listQuery.pageIndex - 1) * this.listQuery.pageSize + 1;
      },
    search(){
      this.fetchData()
    },
      add(){
      this.resetTemp()
      this.temp.type=1
      this.dialogStatus = 'add'
      this.dialogVisible = true
    },
    resetTemp() {
      this.temp = {
        domainAccount:'',
        realName:'',
        description:''
      }
    },
     openFileDialog(row) {
       this.temp= Object.assign({}, row)
       this.temp.isUse=row.isUse+''
      this.dialogVisible = true
       this.dialogStatus = 'update'
    },
    save(){
      if(this.dialogStatus == 'add')
      {
         addProject(this.temp).then(response => {
            this.dialogVisible=false
            this.$message({
             message: '添加成功',
             type: 'success'
              });
          })
      }
      else if(this.dialogStatus == 'update')
      {
          updateProject(this.temp).then(response => {
            this.dialogVisible=false
            this.$message({
             message: '修改成功',
             type: 'success'
              });
          })
      }
       this.fetchData()
    },
    fetchData() {
      this.listLoading = true
      getProjectList(this.listQuery).then(response => {
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
