using System.Collections.Generic;

namespace IDC_WEB
{
    public static class CommandString
    {
        public static List<string> lstMarketDetail = new List<string>() { "A", "CBOT", "CCFX", "COMEX", "HK", "LME", "N", "NYMEX","O", "SS", "SZ", "XDCE", "XSGE", "XZCE" };
        public static string marketDetail = "SS,SZ,HK,A,N,O,CBOT,CCFX,NYMEX,COMEX,LME,XDCE,XSGE,XZCE";
        public static string DefaultFields = "preclose_px,last_px,business_amount,business_balance,open_px,high_px, low_px,offer_grp,bid_grp";
        /// <summary>
        /// 基础接口
        /// </summary>
        //private static string baseUrl = "http://open.hundsun.com:8081/quote/v1/";
        private static string baseUrl = "http://open.hs.net/quote/v1/";
        /// <summary>
        /// 市场列表
        /// Return：finace_mic-交易所识别码；finace_name-交易所名称
        /// </summary>
        public static string Market_List = baseUrl + "market/list";
        /// <summary>
        /// 年度节假日
        /// </summary>
        public static string Market_Holiday = baseUrl + "market/holiday";
        /// <summary>
        /// 市场详细信息
        /// </summary>
        public static string Market_Detail = baseUrl + "market/detail";
        /// <summary>
        /// 行情报价
        /// </summary>
        public static string Real = baseUrl + "real";
        /// <summary>
        /// 分时线
        /// </summary>
        public static string Trend = baseUrl + "trend";
        /// <summary>
        /// K线
        /// </summary>
        public static string Kline = baseUrl + "kline";
        /// <summary>
        /// 排序的行情报价
        /// </summary>
        public static string Sort = baseUrl + "sort";
        /// <summary>
        /// 键盘精灵
        /// </summary>
        public static string Wizard = baseUrl + "wizard";
        /// <summary>
        /// 市场年度节假日（指定市场）
        /// Eg：http://[baseURL]/market/holiday?finance_mic=SS
        /// </summary>
        /// <param name="finaceMic">交易所识别码</param>
        /// <returns>finance_mic：交易所识别码，en_holiday：假日列表</returns>
        public static string GetMarketHoliday(string finaceMic)
        {
            return string.Format("{0}?finace_mic={1}", Market_Holiday, finaceMic).Replace(" ", "");
        }
        /// <summary>
        /// 市场年度节假日（指定市场和日期）
        /// Eg：http://[baseURL]/market/holiday?finance_mic=SS&date=2012
        /// </summary>
        /// <param name="finaceMic">交易所识别码</param>
        /// <param name="date">日期</param>
        /// <returns>finance_mic：交易所识别码，en_holiday：假日列表</returns>
        public static string GetMarketHoliday(string finaceMic, string date)
        {
            return string.Format("{0}?finace_mic={1}&date={2}", Market_Holiday, finaceMic, date).Replace(" ", "");
        }
        /// <summary>
        /// 市场详细信息
        /// </summary>
        /// <param name="finaceMic">交易所识别码</param>
        /// <returns></returns>
        public static string GetMarketDetail(string finaceMic)
        {
            return string.Format("{0}?finance_mic={1}", Market_Detail, finaceMic).Replace(" ", "");
        }
        /// <summary>
        /// 行情报价
        /// </summary>
        /// <param name="en_prod_code">产品代码集 </param>
        /// <returns></returns>
        public static string GetReal(string en_prod_code)
        {
            return string.Format("{0}?en_prod_code={1}", Real, en_prod_code).Replace(" ", "");
        }
        /// <summary>
        /// 行情报价
        /// </summary>
        /// <param name="en_prod_code">产品代码集 </param>
        /// <param name="fields">字段集合</param>
        /// <returns></returns>
        public static string GetReal(string en_prod_code, string fields)
        {
            return string.Format("{0}?en_prod_code={1}&fields={2}", Real, en_prod_code, fields).Replace(" ", "");
        }
        /// <summary>
        /// 分时
        /// </summary>
        /// <param name="prod_code">股票代码</param>
        /// <returns></returns>
        public static string GetTrend(string prod_code)
        {
            return string.Format("{0}?prod_code={1}", Trend, prod_code).Replace(" ", "");
        }
        /// <summary>
        /// 分时
        /// </summary>
        /// <param name="prod_code">股票代码</param>
        /// <param name="fields">字段集合</param>
        /// <returns></returns>
        public static string GetTrend(string prod_code, string fields)
        {
            return string.Format("{0}?prod_code={1}&fields={2}", Trend, prod_code, fields).Replace(" ", "");
        }
        /// <summary>
        /// 分时
        /// </summary>
        /// <param name="prod_code">股票代码</param>
        /// <param name="fields">字段集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static string GetTrend(string prod_code, string fields, string date)
        {
            return string.Format("{0}?prod_code={1}&fields={2}&date={3}", Trend, prod_code, fields, date).Replace(" ", "");
        }

        public static string GetWizard(string prod_code)
        {
            return string.Format("{0}?prod_code={1}&en_finance_mic={2}", Wizard, prod_code, marketDetail).Replace(" ", "");
        }
        public static string GetSort(string en_hq_type_code, string sort_field_name)
        {
            return string.Format("{0}?en_hq_type_code={1}&sort_field_name={2}", Sort, en_hq_type_code, sort_field_name).Replace(" ", "");
        }
    }
}