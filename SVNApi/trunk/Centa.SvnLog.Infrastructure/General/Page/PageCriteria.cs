
namespace Centa.SvnLog.Infrastructure.General.Page
{
    /// <summary>
    /// 分页数据查询设置
    /// </summary>
    public class PageCriteria
    {
        private string _tableName;

        /// <summary>
        /// 表名，也可以是视图名称
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private string _Fileds = "*";

        /// <summary>
        /// 查询字段
        /// </summary>
        public string Fields
        {
            get { return _Fileds; }
            set { _Fileds = value; }
        }

        private string _primaryKey = "ID";

        /// <summary>
        /// 主键，会用这个字段排序
        /// </summary>
        public string PrimaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }

        private int _pageSize = 10;

        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int _currentPage = 1;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        private string _sort = string.Empty;

        /// <summary>
        /// 排序，ASC和DESC
        /// </summary>
        public string Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        private string _condition = string.Empty;

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        private int _recordCount;

        /// <summary>
        /// 记录数
        /// </summary>
        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }
    }
}