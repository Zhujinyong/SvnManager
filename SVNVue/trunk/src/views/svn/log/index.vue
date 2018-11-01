<template>
  <div class="app-container">
     <div class="filter-container">
      <el-select v-model="logListQuery.projectID" filterable clearable placeholder="项目"  @change="resetPageIndex"  >
      <el-option
      v-for="item in projectList"
      :key="item.id"
      :label="item.name"
      :value="item.id">
    </el-option>
    </el-select>
    <el-select v-model="logListQuery.userID" filterable clearable placeholder="姓名"  @change="resetPageIndex" >
    <el-option
      v-for="item in svnUserList"
      :key="item.id"
      :label="item.realName"
      :value="item.id">
    </el-option>
    </el-select>
    <el-date-picker
      v-model="time"
      type="datetimerange"
      :picker-options="pickerOptions"
      range-separator="至"
      start-placeholder="开始时间"
      end-placeholder="结束时间"
      align="right"
      @change="resetPageIndex" >
    </el-date-picker>
    <el-button type="primary" icon="el-icon-search" @click="getLogListByCondition">查询</el-button>
    </div>

    <el-table  v-loading="listLoading"  border fit highlight-current-row :data="logList" style="margin-top:20px;width: 100%">
   <el-table-column align="center" label="序号" width="60"  type="index"
            :index="typeIndex">
       
      </el-table-column>
    <el-table-column
      label="项目" align="center"
      prop="svnProject.name">
    </el-table-column>
    <el-table-column
      label="版本" align="center"
      prop="revision">
    </el-table-column>
     <el-table-column
      label="提交人" align="center"
      prop="svnUser.realName">
    </el-table-column>
    <el-table-column
      label="提交时间" align="center"
      prop="createTime">
    </el-table-column>
    <el-table-column
      label="备注" align="center"
      prop="message">
    </el-table-column>
     <el-table-column
      fixed="right"
      label="操作" align="center"
      width="200">
      <template slot-scope="scope">
        <el-button @click="openFileDialog(scope.row)"  size="small"  type="primary" >查看</el-button>
         <el-button @click="openPublishDialog(scope.row)"  size="small"  type="primary" >发布</el-button>
      </template>
    </el-table-column>
  </el-table>

  <div class="pagination-container">
      <el-pagination :current-page="logListQuery.pageIndex" :page-sizes="[10,20,30,50]" :page-size="logListQuery.pageSize" :total="total" layout="total, sizes, prev, pager, next, jumper" @size-change="handleSizeChange" @current-change="handleCurrentChange">
      </el-pagination>
      
    </el-pagination>
    </el-pagination>
  </div>
  
   <el-dialog title="变更文件列表" :visible.sync="dialogVisible" :close-on-click-modal="false">
    <el-table   v-loading="logFileListLoading"  border fit highlight-current-row :data="logFileList" style="margin-top:20px;width: 100%">
     <el-table-column align="center" label="序号" width="60">
        <template slot-scope="scope">
          {{ scope.$index+1 }}
        </template>
      </el-table-column>
    <el-table-column  align="center" label="操作" prop="action">
    </el-table-column>
    <el-table-column align="center" label="路径" prop="path">
     </el-table-column>
       <el-table-column
      fixed="right"
      label="操作" align="center"
      width="200">
      <template slot-scope="scope">
        <el-button v-if="scope.row.action=='Modify'" @click="openFileChangeDialog(scope.row,'change')"  size="small"  type="primary" >变更</el-button>
         <el-button  v-if="scope.row.action!='Delete'"  @click="openFileChangeDialog(scope.row,'content')"  size="small"  type="primary" >源文件</el-button>
      </template>
    </el-table-column>
    </el-table>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="dialogVisible=false">关闭</el-button>
      </div>
    </el-dialog>


     <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogChangeVisible" :close-on-click-modal="false" :fullscreen="true">
      

      {{errorMessage}}
      <el-input type="textarea" :rows="20" 
       v-model="fileString"
       v-loading="textLoading"
       >
       </el-input>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="dialogChangeVisible=false">关闭</el-button>
      </div>
    </el-dialog>

 
 <el-dialog title="发布" :visible.sync="dialogPublishVisible" :close-on-click-modal="false" :fullscreen="true">
     <div class="filter-container">
     <el-button type="primary" icon="el-icon-plus"   @click="openPublishFormDialog">发布</el-button>
    </div>
       <el-table
      v-loading="publishListLoading"
      :data="publishList"
      element-loading-text="加载中"
      border
      fit
      highlight-current-row style="margin-top:20px" >
   
      <el-table-column label="配置名称" align="center" width="200">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
       <el-table-column label="发布时间" align="center" width="200">
        <template slot-scope="scope">
          {{ scope.row.createTime }}
        </template>
      </el-table-column>
        <el-table-column label="发布人" align="center" width="200">
        <template slot-scope="scope">
          {{ scope.row.realName }}
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center" >
        <template slot-scope="scope">
          {{ scope.row.description }}
        </template>
      </el-table-column>
     
    </el-table>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="dialogPublishVisible=false">关闭</el-button>
      </div>
    </el-dialog>
 
 <el-dialog :title="textPublishMap[dialogPublishStatus]" :visible.sync="dialogPublishFormVisible" :close-on-click-modal="false">
       <el-form ref="form" :model="tempPublish" label-width="120px">
      <el-form-item label="配置名称">
       <el-select v-model="tempPublish.jenkinsID" filterable clearable placeholder="配置">
      <el-option
      v-for="item in jenkinsList"
      :key="item.id"
      :label="item.name"
      :value="item.id">
      </el-option>
       </el-select>
      </el-form-item>
     <el-form-item label="备注">
      <el-input  type="textarea"  v-model="tempPublish.description" placeholder="请输入"></el-input>
      </el-form-item>
      
    </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="publishToJenkins">发布</el-button>
        <el-button  type="primary" @click="dialogPublishFormVisible=false">关闭</el-button>
      </div>
    </el-dialog>
 
 
 </div>

</template>

<script>
import { getSvnUserList } from '@/api/svn/user'
import { getProjectList } from '@/api/svn/project'
import { getRevisionPublishList,addRevisionPublish } from '@/api/svn/publish'
import { getLogList,getLogFileList,GetFileChange,GetFileContent } from '@/api/svn/log'
import { getSvnJenkinsList } from '@/api/svn/jenkins'

import { parseTime } from '@/utils/index'


export default {
  filters: {
  },
  data() {
    return {
      currentLog:null,
      errorMessage:'',
      textLoading:true,
      dialogStatus: '',
      textMap: {
        content: '源文件',
        change: '变更'
      },
      fileString:'',
      dialogChangeVisible:false,
      dialogVisible:false,
      dialogPublishVisible:false,
      pickerOptions: {
          shortcuts: [{
            text: '最近一周',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit('pick', [start, end]);
            }
          }, {
            text: '最近一个月',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit('pick', [start, end]);
            }
          }, {
            text: '最近三个月',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit('pick', [start, end]);
            }
          }]
        },
      total: null,
      logList: null,
      logFileList: null,
      listLoading: true,
      logFileListLoading: true,
      jenkinsListLoading:true,
      publishList: [],
      dialogPublishStatus:'',
      dialogPublishFormVisible:false,
       tempPublish:{
        id:'',
        logID:'',
        jenkinsID:'',
        description:'',
        createTime:''
      },
      publishListLoading:true,
      svnUserListQuery: {
        name: '',
        pageSize:1000
      },
      projectListQuery: {
        realName: '',
        pageSize:1000
      },
      projectList:null,
      svnUserList:null,
      jenkinsList:null,
      time:undefined,
       textPublishMap: {
        update: '编辑',
        add: '发布'
      },
      logListQuery: {
        pageIndex: 1,
        pageSize: 10,
        beginTime: undefined,
        endTime: undefined,
        projectID: undefined,
        userID: undefined
      },
      logFileListQuery: {
        logID:undefined
      },
      logFileDetailQuery: {
        logFileID:undefined
      },
      publishQuery: {
        log:''
      }
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
     resetPageIndex(v){
      this.logListQuery.pageIndex=1
    },
    publishToJenkins(){
      this.tempPublish.logID=this.currentLog.id
       addRevisionPublish(this.tempPublish).then(response => {
            this.dialogPublishFormVisible=false
            this.getPublishList()
            this.$message({
             message: '发布成功',
             type: 'success'
              });
          })

    },
    openPublishFormDialog(){
      this.resetPublishTemp()
      this.dialogPublishStatus = 'add'
      this.dialogPublishFormVisible = true
      this.getJenkinsList()

    },
    resetPublishTemp() {
      this.tempPublish ={
        id:'',
        jenkinsID:'',
        description:''
      }
    },
      getJenkinsList() {
        this.jenkinsList = null
        this.jenkinsListLoading = true
        getSvnJenkinsList().then(response => {
        this.jenkinsList = response.data
        this.jenkinsListLoading = false
        })
    },
    getPublishList() {
        this.publishList = null
        this.publishListLoading = true
        getRevisionPublishList(this.publishQuery).then(response => {
        this.publishList = response.data
        this.publishListLoading = false
        })
    },
    typeIndex(index) {
        return index + (this.logListQuery.pageIndex - 1) * this.logListQuery.pageSize + 1;
      },
     getLogFileListByLogID() {
        this.logFileList = null
        this.logFileListLoading = true
        getLogFileList(this.logFileListQuery).then(response => {
        this.logFileList = response.data
        this.logFileListLoading = false
        })
    },
     openFileChangeDialog(row,type) {
      this.dialogStatus = type
      this.logFileDetailQuery.logFileID=row.id;
      this.dialogChangeVisible = true
      this.fileString = ''
      this.textLoading=true
      this.errorMessage=''
      const method=type=='change'?GetFileChange:GetFileContent
      method(this.logFileDetailQuery).then(response => {
         this.textLoading=false
          if(response.data.indexOf('{"Content":"')>-1){
             const data=JSON.parse(response.data)
           this.fileString = data.Content
          
          }
          else{
           this.errorMessage=response.data
           }
        }
      )  
    },
      openFileDialog(row) {
      this.logFileListQuery.logID=row.id;
      this.dialogVisible = true
      this.getLogFileListByLogID()
    },
      openPublishDialog(row) {
        this.currentLog=row
      this.publishQuery.logID=row.id;
      this.dialogPublishVisible = true
      this.getPublishList()
    },
    getLogListByCondition() {
        this.listLoading = true
        if (this.time!=null&&typeof(this.time) !== "undefined")
        { 
          this.logListQuery.beginTime=parseTime(this.time[0])
          this.logListQuery.endTime=parseTime(this.time[1])
        }  
         else{
          this.logListQuery.beginTime=''
          this.logListQuery.endTime=''
        }
        getLogList(this.logListQuery).then(response => {
        this.logList = response.data
        this.listLoading = false
        this.total=response.page.total
        })
    },
    fetchData() {
      getSvnUserList(this.svnUserListQuery).then(response => {
        this.svnUserList = response.data
        getProjectList(this.projectListQuery).then(response => {
             this.projectList = response.data
             this.getLogListByCondition()
          })
      })
      
    },
    handleSizeChange(val) {
      this.logListQuery.pageSize = val
      this.fetchData()
    },
    handleCurrentChange(val) {
      this.logListQuery.pageIndex = val
      this.fetchData()
    }
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>


</style>

