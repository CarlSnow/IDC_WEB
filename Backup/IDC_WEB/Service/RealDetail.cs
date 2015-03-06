using System.Collections.Generic;

namespace IDC_WEB
{
    public class RealDetail
    {
        public List<RealInfo> lstRealInfo = new List<RealInfo>();
    }

    /// <summary>
    /// 实时行情数据包
    /// </summary>
    public class RealInfo
    {
        /// <summary>
        /// 证券代码
        /// </summary>
        public string secucode;
        /// <summary>
        /// 证券简称
        /// </summary>
        public string secuabbr;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string data_timestamp;
        /// <summary>
        /// 每手股数
        /// </summary>
        public int shares_per_hand;
        /// <summary>
        /// 昨收盘价
        /// </summary>
        public double preclose_px;
        /// <summary>
        /// 开盘价
        /// </summary>
        public double open_px;
        /// <summary>
        /// 最新价
        /// </summary>
        public double last_px;
        /// <summary>
        /// 最高价
        /// </summary>
        public double high_px;
        /// <summary>
        /// 最低价
        /// </summary>
        public double low_px;
        /// <summary>
        /// 成交量 
        /// </summary>
        public double business_amount;
        /// <summary>
        /// 成交额
        /// </summary>
        public double business_balance;
        /// <summary>
        /// 委买档位
        /// </summary>
        public string bid_grp;
        /// <summary>
        /// 委卖档位 
        /// </summary>
        public string offer_grp;
        /// <summary>
        /// 涨跌
        /// </summary>
        public double pct_change;
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public double pct_change_rate;
    }
}