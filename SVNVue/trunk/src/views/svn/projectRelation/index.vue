<template>
  <div class="app-container">
    <div class="filter-container">
     
    </div>
     <el-tree :props="props1" node-key="id" :load="loadProjectTree"
       ref="tree"
       
    highlight-current
    :expand-on-click-node="false"
        lazy >
    
      <span class="custom-tree-node"  slot-scope="{ node, data }">
        <span> <i :class="node.icon"></i>{{ node.label }}</span>
        <span >
          <el-button type="text" icon="el-icon-plus" size="mini" @click="() => addProjectRelation(node,data)">
            添加项目
          </el-button>
           <el-button v-if="data.id!='root'" type="text" icon="el-icon-setting" size="mini" @click="() => deleteProjectRelation(node,data)">
            删除项目
          </el-button>
         <el-button v-if="data.id!='root'" type="text" icon="el-icon-setting" size="mini" @click="() => openJenkins(node,data)">
            发布配置
          </el-button>
        </span>
      </span>
      </el-tree>

     <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogVisible" :close-on-click-modal="false">
       <el-form ref="form" :model="temp" label-width="120px">
   
      <el-form-item label="项目">
       <el-select v-model="temp.childID" placeholder="请选择" filterable clearable  @change="changeValue" >
        <el-option v-for="item in projectList" :key="item.id" :label="item.name" :value="item.id">
        </el-option>
       </el-select>
    </el-form-item>
    </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="save">保存</el-button>
        <el-button  type="primary" @click="dialogVisible=false">关闭</el-button>
      </div>
    </el-dialog>

    <el-dialog title="发布配置" :visible.sync="dialogJenkinsVisible" :close-on-click-modal="false" :fullscreen="true">
     <div class="filter-container">
     <el-button type="primary" icon="el-icon-plus"   @click="addJenkins">添加</el-button>
    </div>
       <el-table
      v-loading="listLoading"
      :data="jenkinsList"
      element-loading-text="Loading"
      border
      fit
      highlight-current-row style="margin-top:20px" >
   
      <el-table-column label="配置名称" align="center" width="200">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
       <el-table-column label="Job名称" align="center" width="200">
        <template slot-scope="scope">
          {{ scope.row.jobName }}
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center" >
        <template slot-scope="scope">
          {{ scope.row.description }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="操作" width="250">
      <template slot-scope="scope">
        <el-button @click="editJenkins(scope.row)"  size="small"  type="primary" icon="el-icon-edit" >编辑</el-button>
      </template>
      </el-table-column>
    </el-table>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="dialogJenkinsVisible=false">关闭</el-button>
      </div>
    </el-dialog>

   <el-dialog :title="textJenkinsMap[dialogJenkinsStatus]" :visible.sync="dialogJenkinsEditVisible" :close-on-click-modal="false">
       <el-form ref="form" :model="tempJenkins" label-width="120px">
      <el-form-item label="配置名称">
      <el-input v-model="tempJenkins.name" placeholder="请输入"></el-input>
      </el-form-item>
      <el-form-item label="Job名称">
      <el-input v-model="tempJenkins.jobName" placeholder="请输入"></el-input>
      </el-form-item>
     <el-form-item label="备注">
      <el-input v-model="tempJenkins.description" placeholder="请输入"></el-input>
      </el-form-item>
      
    </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button  type="primary" @click="saveJenkins">保存</el-button>
        <el-button  type="primary" @click="dialogJenkinsEditVisible=false">关闭</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { getProjectRelationTreeList,addProjectRelation,deleteProjectRelation } from '@/api/svn/projectRelation'
import { newGuid } from '@/utils/index'
import { getProjectList } from '@/api/svn/project'
import { getSvnJenkinsList,addSvnJenkins,UpdateSvnJenkins } from '@/api/svn/jenkins'

export default {
  data() {
    return {
      listLoading: true,
      currentNode:null,
      projectList:[],
      jenkinsList:[],
       temp:{
        id:'',
        parentID:'',
        childID:'',
        name:''
      },
       tempJenkins:{
        id:'',
        name:'',
        jobName:'',
        description:'',
        projectRelationID:''
      },
      dialogVisible:false,
      dialogJenkinsVisible:false,
      dialogJenkinsEditVisible:false,
        dialogStatus: '',
        dialogJenkinsStatus: '',
      textMap: {
        update: '编辑',
        add: '添加'
      },
       textJenkinsMap: {
        update: '编辑',
        add: '添加'
      },
      menuWidth:200,
      menuLeft:'',
      menuTop:'',
      menuVisible:false,
        props1: {
          label: 'name',
          children: [],
          isLeaf: 'leaf'
        },
        treeQuery: {
          id: ''
      }

    }
  },
  created() {
  },
  methods: {
    deleteProjectRelation(node,data) {
       this.$confirm('此操作将永久删除'+data.name+', 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
           deleteProjectRelation({id:data.id}).then(response => {
             this.$refs.tree.remove(node);
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
     saveJenkins(){
       alert(this.tempJenkins.projectRelationID)
       return
       this.tempJenkins.projectRelationID=this.currentNode.data.id
      if(this.dialogJenkinsStatus == 'add')
      {
         addSvnJenkins(this.tempJenkins).then(response => {
            this.dialogJenkinsEditVisible=false
            this.$message({
             message: '添加成功',
             type: 'success'
              });
          })
      }
      else if(this.dialogJenkinsStatus == 'update')
      {
          UpdateSvnJenkins(this.tempJenkins).then(response => {
            this.dialogJenkinsEditVisible=false
            this.$message({
             message: '修改成功',
             type: 'success'
              });
          })
      }
       this.getJenkinsList()
    },
     getJenkinsList(){
     this.jenkinsList =[]
       this.listLoading = true
       getSvnJenkinsList({projectRelationID:this.currentNode.data.id }).then(response => {
        this.listLoading = false
        this.jenkinsList = response.data

       })
    },
    addJenkins(){
      this.resetJenkinsTemp()
      this.dialogJenkinsStatus = 'add'
      this.dialogJenkinsEditVisible = true
    },
      editJenkins(row) {
      this.tempJenkins= Object.assign({}, row)
      this.dialogJenkinsEditVisible = true
       this.dialogJenkinsStatus = 'update'
    },
     resetJenkinsTemp() {
      this.tempJenkins ={
        id:'',
        name:'',
        jobName:'',
        description:'',
        projectRelationID:''
      }
    },
     openJenkins(node,data) {
       this.currentNode=node
       this.dialogJenkinsVisible=true
       this.getJenkinsList()
      },
     changeValue(value) {
          let obj = {};
          obj = this.projectList.find((item)=>{
              return item.id === value;
          })
          this.temp.name=obj.name
        },
      save(){
      if(this.dialogStatus == 'add')
      {
        if(this.temp.childID=='')
        {
          return this.$message.error('请选择项目');
        }
        this.temp.id=newGuid()
         addProjectRelation(this.temp).then(response => {
            this.dialogVisible=false
            this.$message({
             message: '添加成功',
             type: 'success'
              });
            this.$refs.tree.append({id:this.temp.id,name:this.temp.name},this.temp.parentID);
          })
      }
      else if(this.dialogStatus == 'update')
      {
          
      }
    },
    resetTemp() {
      this.temp = {
        parentID:'',
        childID:''
      }
    },
      addProjectRelation(node,data) {
        this.currentNode=node
       this.resetTemp()
       this.temp.parentID=data.id
       this.dialogStatus = 'add'
       this.dialogVisible=true
       this.projectList =[]
       getProjectList({pageIndex: 1,pageSize: 1000 }).then(response => {
        this.projectList = response.data
       })
      },
      updateProjectRelation(node, data) {
       this.resetTemp()
       this.temp.parentID=data.id
       this.dialogStatus = 'update'
       this.dialogVisible=true
       this.projectList =[]
       getProjectList({pageIndex: 1,pageSize: 1000 }).then(response => {
        this.projectList = response.data
       })
      },
     loadProjectTree(node, resolve) {
        if (node.level === 0) {
          return resolve([{ name: '项目树',id:'root' }]);
        }
        if (node.level > 0) {
           getProjectRelationTreeList({id:node.data.id})
           .then(response => {
                  const data = response.data
                 resolve(data)
                 })
            }
      }
  
  }
}
</script>
<style>
  .custom-tree-node {
    flex: 1;
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 14px;
    padding-right: 8px;
  }
</style>
