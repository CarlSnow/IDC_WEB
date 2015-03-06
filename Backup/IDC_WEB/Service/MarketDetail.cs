
namespace IDC_WEB
{
    public class MarketDetail
    {
        public MarketInfo data;
    }
    public class MarketInfo
    {
        public string finance_mic;
        public string finance_name;
        public string hq_market_date;
        public MarketTradeDate[] trade_section_grp;
        public MarketProd[] market_detail_prod_grp;
    }
    public class MarketTradeDate
    {
        public string open_time;
        public string close_time;
    }
    public class MarketProd
    {
        public string prod_code;
        public string prod_name;
        public string hq_type_code;
    }
}