<template>
  <div class="app-container">
     <div class="filter-container"> 
       <el-row>
  <el-col :span="6">
     <el-select v-model="listQuery.id" filterable clearable placeholder="姓名" >
    <el-option
      v-for="item in svnUserList"
      :key="item.id"
      :label="item.realName"
      :value="item.id">
    </el-option>
    </el-select>
  </el-col>
  <el-col :span="10">
     <el-date-picker
      v-model="time"
      type="datetimerange"
      :picker-options="pickerOptions"
      range-separator="至"
      start-placeholder="开始时间"
      end-placeholder="结束时间"
      align="right"
      >
    </el-date-picker>
  </el-col>
  <el-col :span="8">
    <el-button type="primary" icon="el-icon-search" @click="search">
        查询
       </el-button>
  </el-col>
  </el-row>
    </div>
<el-table   
     v-loading="listLoading"  
     border fit highlight-current-row 
     :data="statisticsSvnUserList" style="margin-top:20px;width: 100%;">
     <el-table-column align="center" label="序号" width="60">
        <template slot-scope="scope">
          {{ scope.$index+1 }}
        </template>
      </el-table-column>
    <el-table-column  align="center" label="提交人" prop="realName">
    </el-table-column>
    <el-table-column  align="center" label="项目" prop="name">
    </el-table-column>
     <el-table-column  align="center" label="提交次数" prop="submitCount">
    </el-table-column>
    <el-table-column align="center" label="开始提交时间" prop="beginTime">
     </el-table-column>
     <el-table-column align="center" label="最后提交时间" prop="endTime">
     </el-table-column>
      <el-table-column
      align="center"
      label="操作"
     >
      <template slot-scope="scope">
        <el-button @click="openLogDialog(scope.row)"  size="small"  type="primary" icon="el-icon-search" >查看日志</el-button>
      </template>
    </el-table-column>
    </el-table>


   <el-dialog title="提交日志" 
    :visible.sync="logDialogVisible" 
    :close-on-click-modal="false"
    :fullscreen="true">
    <el-table  
     v-loading="logListLoading"  
     border fit highlight-current-row 
      height="500"
     :data="logList" style="margin-top:20px;width: 100%;">
      <el-table-column align="center" label="序号" width="60"  type="index"
            :index="typeIndex">
       
      </el-table-column>
      <el-table-column
      label="提交人" align="center"
      prop="svnUser.realName">
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
      label="提交时间" align="center"
      prop="createTime">
    </el-table-column>
    <el-table-column
      label="备注" align="center"
      prop="message">
    </el-table-column>
    </el-table>
     <div class="pagination-container">
      <el-pagination :current-page="logListQuery.pageIndex" :page-sizes="[10,20,30,50]" :page-size="logListQuery.pageSize" :total="total" layout="total, sizes, prev, pager, next, jumper" @size-change="handleSizeChange" @current-change="handleCurrentChange">
      </el-pagination>
      
    </el-pagination>
    </el-pagination>
  </div>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="logDialogVisible=false">关闭</el-button>
      </div>
    </el-dialog>
    </div>

    

  </div>
</template>

<script>
import { getSvnUserList } from '@/api/svn/user'
import { getStatisticsSvnUserList } from '@/api/svn/statistics'
import { parseTime } from '@/utils/index'
import { getLogList} from '@/api/svn/log'

export default {
   name: 'staticUser',
   data() {
    return {
      svnUserList:null,
       svnUserListQuery: {
        name: '',
        pageSize:1000
      },
     logDialogVisible:false,
      time:undefined,
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
     
      statisticsSvnUserList:[],
      listLoading: false,
      logListLoading: false,
      logList:[],
      listQuery: {
        id: null,
        beginTime: '',
        endTime:''
      },
       total: null,
      currentModel:null,
      logListQuery: {
        pageIndex: 1,
        pageSize: 10,
        beginTime: undefined,
        endTime: undefined,
        projectID: undefined,
        userID: undefined
      },
    }
  },
  created() {
    this.loadSvnUserList()
  },
  methods: {
    typeIndex(index) {
        return index + (this.logListQuery.pageIndex - 1) * this.logListQuery.pageSize + 1
      },
     handleSizeChange(val) {
      this.logListQuery.pageSize = val
      this.loadLogList()
    },
    handleCurrentChange(val) {
      this.logListQuery.pageIndex = val
      this.loadLogList()
    },
    loadLogList(){
       if (this.time!=null&&typeof(this.time) !== "undefined")
        { 
          this.logListQuery.beginTime=parseTime(this.time[0])
          this.logListQuery.endTime=parseTime(this.time[1])
        } 
          else{
          this.logListQuery.beginTime=''
          this.logListQuery.endTime=''
        }
         this.logListQuery.projectID=this.currentModel.projectID
         this.logListQuery.userID=this.currentModel.userID
         this.logListLoading=true
         getLogList(this.logListQuery).then(response => {
        this.logListLoading=false
        this.logList = response.data
        this.total=response.page.total

    })
    },
     openLogDialog(row) {
        this.currentModel=row
        this.logDialogVisible = true
         this.logListQuery.pageSize=10
         this.logListQuery.pageIndex = 1
        this.loadLogList()
    },
    loadSvnUserList(){
       getSvnUserList(this.svnUserListQuery).then(response => {
        this.svnUserList = response.data
      })
    },
    search(){
      if(this.listQuery.id==null||typeof(this.listQuery.id) == "undefined"||this.listQuery.id==''){
        return  this.$message({
             message: '请选择提交人',
             type: 'warning'
              });
      }
       this.listLoading=true
       if (this.time!=null&&typeof(this.time) !== "undefined")
        { 
          this.listQuery.beginTime=parseTime(this.time[0])
          this.listQuery.endTime=parseTime(this.time[1])
        } 
        else{
          this.listQuery.beginTime=''
          this.listQuery.endTime=''
        }
       getStatisticsSvnUserList(this.listQuery).then(response => {
        this.listLoading=false
        this.statisticsSvnUserList = response.data
      })
    }
  }
}
</script>

