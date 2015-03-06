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

    public static class RealExtension
    {
        public static RealInfo Copy(this RealInfo realInfo)
        {
            RealInfo m_realInfo = new RealInfo();
            m_realInfo.secucode = realInfo.secucode;
            m_realInfo.secuabbr = realInfo.secuabbr;
            m_realInfo.data_timestamp = realInfo.data_timestamp;
            m_realInfo.shares_per_hand = realInfo.shares_per_hand;
            m_realInfo.preclose_px = realInfo.preclose_px;
            m_realInfo.open_px = realInfo.open_px;
            m_realInfo.last_px = realInfo.last_px;
            m_realInfo.high_px = realInfo.high_px;
            m_realInfo.low_px = realInfo.low_px;
            m_realInfo.business_amount = realInfo.business_amount;
            m_realInfo.business_balance = realInfo.business_balance;
            m_realInfo.bid_grp = realInfo.bid_grp;
            m_realInfo.offer_grp = realInfo.offer_grp;
            m_realInfo.pct_change = realInfo.pct_change;
            m_realInfo.pct_change_rate = realInfo.pct_change_rate;
            return m_realInfo;
        }
    }
}