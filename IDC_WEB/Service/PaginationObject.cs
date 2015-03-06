using System.Collections.Generic;

namespace IDC_WEB
{
    /// <summary>
    /// 分页数据结构
    /// </summary>
    public class PaginationObject
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int totalCount;
        /// <summary>
        /// 分页元素序列
        /// </summary>
        public List<RealInfo> items;
    }
}