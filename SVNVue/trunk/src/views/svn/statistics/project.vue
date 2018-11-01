<template>
  <div class="app-container">
     <div class="filter-container"> 
       <el-row>
  <el-col :span="8">
    <treeselect v-model="listQuery.ids"
     :multiple="true" 
     :options="projectTreeList" 
     style="width:300px" 
     noChildrenText=""
     placeholder="选择项目" 
     />
  </el-col>
  <el-col :span="10">
     <el-date-picker
      v-model="time"
      type="datetimerange"
      :picker-options="pickerOptions"
      range-separator="至"
      start-placeholder="开始时间"
      end-placeholder="结束时间"
      align="right">
    </el-date-picker>
  </el-col>
  <el-col :span="6">
    <el-button type="primary" icon="el-icon-search" @click="search">
        查询
       </el-button>
  </el-col>
  </el-row>
    </div>
     <tree-table :data="statisticsProjectTreeList" :columns="columns" :loading="listLoading" border></tree-table>
   <el-dialog title="提交人" 
    :visible.sync="submitPersonDialogVisible" 
    :close-on-click-modal="false"
    :fullscreen="true">
    <el-table  
     v-loading="submitPersonListLoading"  
     border fit highlight-current-row 
      height="500"
     :data="submitPersonList" style="margin-top:20px;width: 100%;">
     <el-table-column align="center" label="序号" width="60">
        <template slot-scope="scope">
          {{ scope.$index+1 }}
        </template>
      </el-table-column>
    <el-table-column  align="center" label="提交人" prop="realName">
    </el-table-column>
     <el-table-column  align="center" label="提交次数" prop="submitCount">
    </el-table-column>
    <el-table-column align="center" label="开始提交时间" prop="beiginTime">
     </el-table-column>
     <el-table-column align="center" label="最后提交时间" prop="endTime">
     </el-table-column>
    </el-table>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="submitPersonDialogVisible=false">关闭</el-button>
      </div>
    </el-dialog>
    </div>
  </div>
</template>

<script>
import { getProjectTreeList } from '@/api/svn/projectRelation'
import { getStatisticsProjectList,getProjectSvnUserList } from '@/api/svn/statistics'
import treeTable from '@/components/TreeTable'
import { parseTime } from '@/utils/index'

export default {
   name: 'statisticsProject',
  components: { treeTable },
  data() {
    var that=this
    return {
      submitPersonDialogVisible:false,
      submitPersonListLoading:true,
      submitPersonList:null,
       columns: [
        {
          text: '项目',
          value: 'label',
          width: 200,
          align:'left'
        },
         {
          text: '提交次数',
          value: 'revisionCount',
           width: 100,
          align:'center'
        },
        {
          text: '开始提交时间',
          value: 'beginTime',
          width: 180,
          align:'center'
        },
        {
          text: '最后提交时间',
          value: 'endTime',
          width: 180,
          align:'center'
        },
         {
          text: '提交人',
          list: 'submitList',
          align:'center',
          operate:
            {
              text:'更多',
              func:function(row){
      that.submitPersionListQuery.id=row.id;
      that.submitPersonDialogVisible = true
      that.getProjectSvnUserList()
              }
            }
        }
      ],
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
      projectValue:null,
      projectTreeList:null,
      statisticsProjectTreeList:[],
      listLoading: true,
      listQuery: {
        ids: null,
        beginTime: '',
        endTime:''
      },
      submitPersionListQuery:{
        id:'',
        beginTime: '',
        endTime:''
      }
    }
  },
  created() {
    this.loadProjectTreeList()
    this.fetchData()
  },
  methods: {
    getProjectSvnUserList(){
       this.submitPersonListLoading=true
       if (this.time!=null&&typeof(this.time) !== "undefined")
        { 
          this.submitPersionListQuery.beginTime=parseTime(this.time[0])
          this.submitPersionListQuery.endTime=parseTime(this.time[1])
        } 
         else{
          this.submitPersionListQuery.beginTime=''
          this.submitPersionListQuery.endTime=''
        }
       getProjectSvnUserList(this.submitPersionListQuery).then(response => {
        this.submitPersonListLoading=false
        this.submitPersonList = response.data
      })
    },
    search(){
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
       getStatisticsProjectList(this.listQuery).then(response => {
        this.listLoading=false
        this.statisticsProjectTreeList = response.data
      })
    },
    fetchData(){
       this.search()
    },
    loadProjectTreeList() {
      getProjectTreeList().then(response => {
        this.projectTreeList = response.data
      })
    }
  }
}
</script>

