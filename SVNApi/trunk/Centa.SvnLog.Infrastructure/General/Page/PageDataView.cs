using System.Collections.Generic;

namespace Centa.SvnLog.Infrastructure.General.Page
{
    /// <summary>
    /// 数据分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataView<T> where T : class
    {
        private int _totalNum;

        public PageDataView()
        {
            _items = new List<T>();
        }

        /// <summary>
        /// 总数据量
        /// </summary>
        public int TotalNum
        {
            get { return _totalNum; }
            set { _totalNum = value; }
        }

        private IList<T> _items;

        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPageCount { get; set; }
    }
}
