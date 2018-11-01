using Centa.SvnLog.ApplicationService.Interface;
using Centa.SvnLog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Centa.SvnLog.ApplicationService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<dynamic> _repository;

        private readonly int _topNumber=5;

        public StatisticsService(IRepository<dynamic> repository)
        {
            _repository = repository;
        }


        public List<dynamic> GetProjectList(List<string> ids, DateTime? beginTime, DateTime? endTime)
        {
            StringBuilder sb = new StringBuilder(@"with tree as(select ChildID,ID,ParentID from SVN_ProjectRelation ");
            if(ids.Count == 0||(ids.Count == 1 && ids[0] == null))
            {
                sb.Append(@"where ParentID IS NULL OR ParentID = ''");
            }
            else
            {
                List<string> whereCondition = new List<string>();
                foreach(var p in ids)
                {
                    whereCondition.Add($"ID= '{p}'");
                }
                var whereString=String.Join(" or ", whereCondition);
                sb.Append($@"where {whereString} ");
            }
            sb.Append(@"union all
            select a.ChildID,a.ID,a.ParentID from SVN_ProjectRelation a  join tree b on a.ParentID = b.id
            )select tree.ID , tree.ChildID ,tree.ParentID, p.Name,case p.Type when 1 then '源代码' when 2 then '产品文档' else '其他' end as Type,p.Head from tree LEFT JOIN SVN_Project p on ChildID = p.ID");
            var sourceList = _repository.Query(sb.ToString()).ToList();
            //叶子节点数据集合
            var leafDataList = GetLeafDataList(sourceList,beginTime,endTime);
            List<dynamic> list=new List<dynamic>();
            if(ids.Count == 0 || (ids.Count == 1 && ids[0] == null))
            {
                FillRecursively(ref sourceList, leafDataList, out list, "", beginTime, endTime);
                list.ForEach(p => {
                    if (p.SubmitList == null || p.SubmitList.Count == 0)
                    {
                        List<dynamic> submitList = new List<dynamic>();
                        foreach (var submit in p.Children)
                        {
                            if (submit.SubmitList == null || submit.SubmitList.Count == 0)
                            {

                            }
                            else
                            {
                                submitList.AddRange(submit.SubmitList);
                            }
                        }
                        p.SubmitList = submitList.GroupBy(d => d.RealName).Select(group => new
                        {
                            RealName = group.Key,
                            EndTime = group.Max(f => f.EndTime)
                        }).OrderByDescending(d => d.EndTime).Take(_topNumber).ToList();
                    }
                });
            }
            else
            {
                foreach (var id in ids)
                {
                    List<dynamic> idList = new List<dynamic>();
                    FillRecursively(ref sourceList, leafDataList, out idList, id, beginTime, endTime);
                    var current =sourceList.FirstOrDefault(p => p.ID == id);
                    if(current!=null)
                    {
                        dynamic project = new ExpandoObject();
                        project.Label = current.Name;
                        project.Type = current.Type;
                        project.Head = current.Head;
                        project.ID = current.ID;
                        if(idList.Count==0)
                        {
                            var currentProject=leafDataList.FirstOrDefault(p => p.ProjectID == current.ChildID);
                            if(currentProject!=null)
                            {
                                project.RevisionCount = currentProject.RevisionCount;
                                project.BeginTime = currentProject.BeginTime;
                                project.EndTime = currentProject.EndTime;
                                var submitList=currentProject.submitList as List<dynamic>;
                                project.SubmitList = submitList.Select(p => new
                                {
                                    RealName = p.RealName,
                                    EndTime = p.EndTime
                                }).ToList();
                            }
                            
                        }
                        else
                        {
                            project.Children = idList;
                            project.RevisionCount = idList.Sum(p => p.RevisionCount);
                            project.BeginTime = idList.Min(p => p.BeginTime);
                            project.EndTime = idList.Max(p => p.EndTime);
                            List<dynamic> submitList = new List<dynamic>();
                            foreach (var submit in idList)
                            {
                                if (submit.SubmitList == null|| submit.SubmitList.Count==0)
                                {
                                   
                                }
                                else
                                {
                                    submitList.AddRange(submit.SubmitList);
                                  
                                }
                            }
                            project.SubmitList = submitList.GroupBy(p => p.RealName).Select(group => new
                            {
                                RealName = group.Key,
                                EndTime = group.Max(f => f.EndTime)
                            }).OrderByDescending(p=>p.EndTime).Take(_topNumber).ToList();
                        }
                        
                        list.Add(project);
                    }
                }
            }
            return list;
        }

        private List<dynamic> GetLeafDataList(List<dynamic> source,DateTime? beginTime, DateTime? endTime)
        {
            List<string> _leafIDList = new List<string>();
            List<dynamic> _dataList = new List<dynamic>();
            foreach (var item in source)
            {
                if(!source.Any(p=>p.ParentID==item.ID))
                {
                    _leafIDList.Add(item.ChildID);
                }
            }
            if(_leafIDList.Count>0)
            {
                StringBuilder sb = new StringBuilder($"select ProjectID,count(ID) as RevisionCount,max(CreateTime) as EndTime,min(CreateTime) as BeginTime from  SVN_Log ");
                List<string> whereCondition = new List<string>();
                foreach (var id in _leafIDList)
                {
                    whereCondition.Add($"ProjectID= '{id}'");
                }
                var whereString = String.Join(" or ", whereCondition);
                sb.Append($@"where ({whereString}) ");
                if (beginTime.HasValue)
                {
                    sb.Append($" and CreateTime>='{beginTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
                }
                if (endTime.HasValue)
                {
                    sb.Append($" and CreateTime<'{endTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
                }
                sb.Append($" GROUP BY ProjectID");
                _dataList = _repository.Query(sb.ToString()).ToList();
                sb.Clear();
                whereCondition.Clear();
                sb.Append($@"
select l.ProjectID, u.RealName, l.EndTime
from
(
");
                
                foreach (var id in _leafIDList)
                {
                    var union =new StringBuilder( $@"
select  top {_topNumber} ProjectID, UserID,max(CreateTime) as EndTime
from SVN_Log
where ProjectID = '{id}' ");
                   
                    if (beginTime.HasValue)
                    {
                        union.Append($" and CreateTime>='{beginTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
                    }
                    if (endTime.HasValue)
                    {
                        union.Append($" and CreateTime<'{endTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
                    }
                    union.Append(" GROUP BY UserID, ProjectID ORDER BY EndTime desc ");
                    whereCondition.Add(union.ToString());
                }

               
                whereString = String.Join(" union all ", whereCondition);
                sb.Append($" {whereString} ");
               
                sb.Append(@"
) as l
LEFT JOIN SVN_User as u on l.UserID = u.ID");
                var submitList = _repository.Query(sb.ToString()).ToList();
                _dataList.ForEach(data => {
                    data.submitList = submitList.Where(p => p.ProjectID == data.ProjectID).ToList();
                });
            }
            return _dataList;
        }

        private void FillRecursively(ref List<dynamic> source, List<dynamic> dataList, out List<dynamic> outValue, string currentID,DateTime? beginTime, DateTime? endTime)
        {
            var matches =string.IsNullOrEmpty(currentID)?source.Where(r => string.IsNullOrEmpty(r.ParentID)).ToArray(): source.Where(r => r.ParentID == currentID).ToArray();
            source.RemoveAll(r => r.ParentID == currentID);
            outValue = new List<dynamic>();
            foreach (var row in matches)
            {
                dynamic project = new ExpandoObject();
                project.Label = row.Name;
                project.Type = row.Type;
                project.Head = row.Head;
                project.ID = row.ID;
                var newChildren = new List<dynamic>();
                FillRecursively(ref source, dataList,out newChildren, project.ID,beginTime,endTime);
                if(newChildren.Count>0)
                {
                    project.Children = newChildren;
                    project.RevisionCount = newChildren.Sum(p => p.RevisionCount);
                    project.BeginTime = newChildren.Min(p => p.BeginTime);
                    project.EndTime = newChildren.Max(p => p.EndTime);
                    List<dynamic> submitList = new List<dynamic>();
                    foreach (var submit in newChildren)
                    {
                        if (submit.SubmitList == null || submit.SubmitList.Count == 0)
                        {

                        }
                        else
                        {
                            submitList.AddRange(submit.SubmitList);
                        }
                    }
                    project.SubmitList = submitList.GroupBy(p => p.RealName).Select(group => new
                    {
                        RealName = group.Key,
                        EndTime = group.Max(f => f.EndTime)
                    }).OrderByDescending(p => p.EndTime).Take(_topNumber).ToList();


                }
                else
                {
                    if(dataList.Count>0)
                    {
                        var data=dataList.FirstOrDefault(p=>p.ProjectID==row.ChildID);
                        if(data!=null)
                        {
                            project.BeginTime = data.BeginTime;
                            project.EndTime = data.EndTime;
                            project.RevisionCount = data.RevisionCount;
                            project.SubmitList = data.submitList;
                        }
                        else
                        {
                            project.BeginTime = null;
                            project.EndTime = null;
                            project.RevisionCount = 0;
                            project.SubmitList = new List<dynamic>();
                        }
                    }
                }
                outValue.Add(project);
            }
        }


        public List<dynamic> GetProjectSvnUserList(string id, DateTime? beginTime, DateTime? endTime)
        {
           var projectTree= _repository.Query($@"with tree as(select ChildID,ID,ParentID from SVN_ProjectRelation where ID= '{id}' union all
            select a.ChildID,a.ID,a.ParentID from SVN_ProjectRelation a  join tree b on a.ParentID = b.id
            )select tree.ID , tree.ChildID ,tree.ParentID, p.Name
from tree LEFT JOIN SVN_Project p on ChildID = p.ID");
            List<string> _leafIDList = new List<string>();
            foreach (var item in projectTree)
            {
                if (!projectTree.Any(p => p.ParentID == item.ID))
                {
                    _leafIDList.Add(item.ID);
                }
            }
            StringBuilder sql = new StringBuilder($@"
select u.RealName,l.SubmitCount,l.BeiginTime,l.EndTime
from
(
select  log.UserID,count(log.ID) as SubmitCount,min(CreateTime) as BeiginTime,max(CreateTime) as EndTime
from SVN_Log as log
LEFT JOIN SVN_ProjectRelation as r on log.ProjectID=r.ChildID
");
            List<string> whereCondition = new List<string>();
            foreach (var pid in _leafIDList)
            {
                whereCondition.Add($"r.ID='{pid}'");
            }
            var whereString = String.Join(" or ", whereCondition);
            sql.Append($@"where ({whereString}) ");
            if (beginTime.HasValue)
            {
                sql.Append($" and log.CreateTime>='{beginTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
            }
            if (endTime.HasValue)
            {
                sql.Append($" and log.CreateTime<'{endTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
            }
            sql.Append(@"GROUP BY log.UserID) as l
LEFT JOIN SVN_User as u on l.UserID = u.ID
ORDER BY l.EndTime desc");
            var list=_repository.Query(sql.ToString());
            return list.ToList();
        }

        public List<dynamic> GetSvnUserList(string id, DateTime? beginTime, DateTime? endTime)
        {
            StringBuilder sql = new StringBuilder($@"
select u.RealName,p.Name,a.UserID,a.ProjectID,a.SubmitCount,a.BeginTime,a.EndTime
from
(select ProjectID,UserID,count(*) as SubmitCount,min(CreateTime) as BeginTime ,max(CreateTime) as EndTime 
from SVN_Log 
where UserID='{id}'
");
            if (beginTime.HasValue)
            {
                sql.Append($" and CreateTime>='{beginTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
            }
            if (endTime.HasValue)
            {
                sql.Append($" and CreateTime<'{endTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}' ");
            }
            sql.Append($@"
GROUP  BY ProjectID,UserID) as a
LEFT JOIN SVN_Project as p on a.ProjectID=p.ID
LEFT JOIN SVN_User as u on a.UserID=u.ID
ORDER BY a.EndTime desc");
            var list = _repository.Query(sql.ToString());
            return list.ToList();
        }
    }
}
