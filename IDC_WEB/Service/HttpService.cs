using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IDC_WEB
{
    public static class HttpService
    {
        private static ManualResetEvent mre = new ManualResetEvent(false);
        private static ManualResetEvent mre_Child = new ManualResetEvent(false);
        private static int threadAliveCount = 0;
        private static ILog m_log = LogManager.GetLogger(typeof(HttpService));
        private static OAuthInfo oAuthInfo = new OAuthInfo();
        private static Hashtable m_MarketRealDetail = new Hashtable();
        private static Hashtable m_SecuCodeRealInfo = new Hashtable();
        private static Thread thread_Main = new Thread(() => Main());
        private static Thread thread_A = new Thread(() => UpdateBaseData("A"));
        private static Thread thread_CBOT = new Thread(() => UpdateBaseData("CBOT"));
        private static Thread thread_CCFX = new Thread(() => UpdateBaseData("CCFX"));
        private static Thread thread_COMEX = new Thread(() => UpdateBaseData("COMEX"));
        private static Thread thread_LME = new Thread(() => UpdateBaseData("LME"));
        private static Thread thread_HK = new Thread(() => UpdateBaseData("HK"));
        private static Thread thread_N = new Thread(() => UpdateBaseData("N"));
        private static Thread thread_NYMEX = new Thread(() => UpdateBaseData("NYMEX"));
        private static Thread thread_O = new Thread(() => UpdateBaseData("O"));
        private static Thread thread_SS = new Thread(() => UpdateBaseData("SS"));
        private static Thread thread_SZ = new Thread(() => UpdateBaseData("SZ"));
        private static Thread thread_XDCE = new Thread(() => UpdateBaseData("XDCE"));
        private static Thread thread_XSGE = new Thread(() => UpdateBaseData("XSGE"));
        private static Thread thread_XZCE = new Thread(() => UpdateBaseData("XZCE"));
        private static Thread thread_MarketInfo = new Thread(InitializeBaseMarketInfo);
        private delegate WizardDetail InitializeWizardDetailHandler(string searchKey);
        private static event InitializeWizardDetailHandler InitializeWizardDetailEventHandler = InitializeBaseWizardInfo;
        private static object lockobj = new object();
        private static System.Timers.Timer refreshTimer = new System.Timers.Timer();
        internal static void Start()
        {
            refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(RefreshTimer_Elapsed);
            refreshTimer.Interval = 60 * 60 * 1000;
            refreshTimer.AutoReset = true;
            refreshTimer.Enabled = true;
            //oAuthInfo = OAuthService.AuthorizationCodeModel();
            //oAuthInfo = OAuthService.ImplicitModel();
            //oAuthInfo = OAuthService.PasswordModel();
            oAuthInfo = OAuthService.ClientModel();
            m_MarketRealDetail.Add("A", new Hashtable());               //纳斯达克
            m_MarketRealDetail.Add("CBOT", new Hashtable());        //芝加哥商品期货
            m_MarketRealDetail.Add("CCFX", new Hashtable());        //中国金融期货交易所
            m_MarketRealDetail.Add("COMEX", new Hashtable());    //纽约商品期货
            m_MarketRealDetail.Add("LME", new Hashtable());         //伦敦金属期货
            m_MarketRealDetail.Add("HK", new Hashtable());           //香港交易所
            m_MarketRealDetail.Add("N", new Hashtable());             //纽约证券交易所
            m_MarketRealDetail.Add("NYMEX", new Hashtable());    //纽约商业期货
            m_MarketRealDetail.Add("O", new Hashtable());             //美国证券交易所
            m_MarketRealDetail.Add("SS", new Hashtable());            //上海证券交易所
            m_MarketRealDetail.Add("SZ", new Hashtable());            //深圳证券交易所
            m_MarketRealDetail.Add("XDCE", new Hashtable());        //大连商品交易所
            m_MarketRealDetail.Add("XSGE", new Hashtable());        //上海期货交易所
            m_MarketRealDetail.Add("XZCE", new Hashtable());        //郑州商品交易所
            thread_MarketInfo.Name = "MarketInfo";
            thread_MarketInfo.Start();
            mre.WaitOne();       //阻止当前线程，直到当前 WaitHandle 收到信号。
            thread_A.Name = "A";
            thread_CBOT.Name = "CBOT";
            thread_CCFX.Name = "CCFX";
            thread_COMEX.Name = "COMEX";
            thread_LME.Name = "LME";
            thread_HK.Name = "HK";
            thread_N.Name = "N";
            thread_NYMEX.Name = "NYMEX";
            thread_O.Name = "O";
            thread_SS.Name = "SS";
            thread_SZ.Name = "SZ";
            thread_XDCE.Name = "XDCE";
            thread_XSGE.Name = "XSGE";
            thread_XZCE.Name = "XZCE";
            thread_Main.Start();
            thread_A.Start();
            thread_CBOT.Start();
            thread_CCFX.Start();
            thread_COMEX.Start();
            thread_LME.Start();
            thread_HK.Start();
            thread_N.Start();
            thread_NYMEX.Start();
            thread_O.Start();
            thread_SS.Start();
            thread_SZ.Start();
            thread_XDCE.Start();
            thread_XSGE.Start();
            thread_XZCE.Start();
        }

        private static void RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (lockobj)
            {
                m_log.Info(string.Format("刷新前通行证：{0}", oAuthInfo.access_token));
                oAuthInfo = OAuthService.RefreshModel(oAuthInfo.refresh_token);
                m_log.Info(string.Format("刷新后通行证：{0}", oAuthInfo.access_token));
            }
        }

        private static void Main()
        {
            try
            {
                while (true)
                {
                    int updateDataTime = 0;
                    lock (lockobj)
                    {
                        threadAliveCount = 14;
                        mre_Child.Set();
                        mre.Reset();                    //将事件状态设置为非终止状态，导致线程阻止。
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        mre.WaitOne(10000);
                        sw.Stop();
                        updateDataTime = (int)sw.ElapsedMilliseconds;
                        m_log.Info(string.Format("后台数据刷新时间：{0}", updateDataTime));
                    }
                    if (updateDataTime < 10000)
                        Thread.Sleep(10000 - updateDataTime);
                }
            }
            catch (Exception ex)
            {
                m_log.Error(Thread.CurrentThread.Name + ":" + ex.ToString());
            }
        }
        private static void UpdateBaseData(string finance_mic)
        {
            try
            {
                while (true)
                {
                    m_log.Info(Thread.CurrentThread.Name + "-Start");
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    InitializeBaseRealInfo(finance_mic);
                    sw.Stop();
                    threadAliveCount--;
                    m_log.Info(string.Format("{0}-End；耗时:{1}；剩余线程数：{2}", Thread.CurrentThread.Name, sw.ElapsedMilliseconds.ToString(), threadAliveCount));
                    if (threadAliveCount == 0)
                        mre.Set();
                    mre_Child.Reset();
                    mre_Child.WaitOne();
                }
            }
            catch (Exception ex)
            {
                m_log.Error(Thread.CurrentThread.Name + ":" + ex.ToString());
            }
        }

        private static string InitializeJson(string url)
        {
            string responseStr = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                request.Headers["Authorization"] = "Bearer " + oAuthInfo.access_token;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                byte[] bytes = new byte[response.ContentLength];
                int position = 0;
                int length = bytes.Length;
                while (true)
                {
                    int len = stream.Read(bytes, position, length);
                    position += len;
                    length -= len;
                    if (position == bytes.Length) break;
                }
                UTF8Encoding converter = new UTF8Encoding();
                responseStr = converter.GetString(bytes);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.ToString());
                return responseStr;
            }
            return responseStr;
        }
        private static void InitializeBaseMarketInfo()
        {
            try
            {
                foreach (string finance_mic in CommandString.lstMarketDetail)
                {
                    Hashtable Category_RealDetail = (Hashtable)m_MarketRealDetail[finance_mic];
                    string marketDetailJson = HttpService.InitializeJson(CommandString.GetMarketDetail(finance_mic));
                    MarketDetail marketDetail = (MarketDetail)JsonConvert.DeserializeObject(marketDetailJson, typeof(MarketDetail));
                    MarketProd[] marketProdArrary = marketDetail.data.market_detail_prod_grp;
                    IEnumerable<IGrouping<string, MarketProd>> enumerable = marketProdArrary.GroupBy(item => item.hq_type_code);
                    foreach (IGrouping<string, MarketProd> marketProdGroup in enumerable)
                    {
                        string en_prod_code = string.Empty;
                        BaseDataObject baseDataObj = new BaseDataObject();
                        int count = 0;
                        foreach (MarketProd marketProd in marketProdGroup)
                        {
                            string secu_code = marketProd.prod_code + "." + finance_mic;
                            if (!baseDataObj.dicSecuInfo.ContainsKey(secu_code))
                                baseDataObj.dicSecuInfo.Add(secu_code, marketProd.prod_name);
                            else
                                baseDataObj.dicSecuInfo[secu_code] = marketProd.prod_name;
                            en_prod_code += secu_code + ",";
                            count++;
                            if (count >= 500)
                            {
                                en_prod_code = en_prod_code.TrimEnd(',');
                                baseDataObj.lstSecuCode.Add(en_prod_code);
                                en_prod_code = string.Empty;
                                count = 0;
                            }
                        }
                        en_prod_code = en_prod_code.TrimEnd(',');
                        baseDataObj.lstSecuCode.Add(en_prod_code);
                        if (!Category_RealDetail.ContainsKey(marketProdGroup.Key.ToUpper()))
                            Category_RealDetail.Add(marketProdGroup.Key.ToUpper(), baseDataObj);
                        else
                            Category_RealDetail[marketProdGroup.Key.ToUpper()] = baseDataObj;
                    }
                }
            }
            catch (Exception ex)
            {
                m_log.Error(Thread.CurrentThread.Name + ":" + ex.ToString());
            }
            finally
            {
                mre.Set();
            }
        }
        private static WizardDetail InitializeBaseWizardInfo(string searchKey)
        {
            Task<WizardDetail> task = new Task<WizardDetail>(n => InitializeBaseWizardInfoTask((string)n), searchKey);
            task.Start();
            return task.Result;
        }
        private static WizardDetail InitializeBaseWizardInfoTask(string searchKey)
        {
            WizardDetail wizardDetail = new WizardDetail();
            try
            {
                string wizardDataDetailJson = HttpService.InitializeJson(CommandString.GetWizard(searchKey));
                wizardDetail = (WizardDetail)JsonConvert.DeserializeObject(wizardDataDetailJson, typeof(WizardDetail));
            }
            catch (Exception ex)
            {
                m_log.Error(ex.ToString());
                return wizardDetail;
            }
            return wizardDetail;
        }
        private static void InitializeBaseRealInfo(string finance_mic)
        {
            try
            {
                Hashtable Category_RealDetail = (Hashtable)m_MarketRealDetail[finance_mic];
                foreach (string key in Category_RealDetail.Keys)
                {
                    BaseDataObject baseDataObj = (BaseDataObject)Category_RealDetail[key];
                    List<Task<RealDetail>> lstTask = new List<Task<RealDetail>>();
                    foreach (string en_prod_code in baseDataObj.lstSecuCode)
                    {
                        SingleBaseDataObject sinleBaseDataObj = new SingleBaseDataObject();
                        sinleBaseDataObj.en_prod_code = en_prod_code;
                        sinleBaseDataObj.dicSecuInfo = baseDataObj.dicSecuInfo;
                        Task<RealDetail> task = new Task<RealDetail>(n => InitializeBaseRealInfoTask((SingleBaseDataObject)n), sinleBaseDataObj);
                        task.Start();
                        lstTask.Add(task);
                    }
                    List<RealInfo> lstRealInfo = new List<RealInfo>();
                    foreach (Task<RealDetail> task in lstTask)
                    {
                        lstRealInfo.AddRange(task.Result.lstRealInfo);
                    }
                    foreach (RealInfo realInfo in lstRealInfo)
                    {
                        Hashtable hash = baseDataObj.m_realInfo;
                        if (hash.ContainsKey(realInfo.secucode))
                            hash[realInfo.secucode] = realInfo;
                        else
                            hash.Add(realInfo.secucode, realInfo);
                        if (m_SecuCodeRealInfo.ContainsKey(realInfo.secucode))
                            m_SecuCodeRealInfo[realInfo.secucode] = realInfo;
                        else
                            m_SecuCodeRealInfo.Add(realInfo.secucode, realInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                m_log.Error(Thread.CurrentThread.Name + ":" + ex.ToString());
                return;
            }
        }
        private static RealDetail InitializeBaseRealInfoTask(SingleBaseDataObject sinleBaseDataObj)
        {
            RealDetail realDetail = new RealDetail();
            try
            {
                sinleBaseDataObj.realJson = HttpService.InitializeJson(CommandString.GetReal(sinleBaseDataObj.en_prod_code, CommandString.DefaultFields));
                JObject realObj = (JObject)JsonConvert.DeserializeObject(sinleBaseDataObj.realJson);
                JObject snapshotObj = (JObject)realObj["data"]["snapshot"];
                foreach (var obj in snapshotObj)
                {
                    RealInfo realInfo = new RealInfo();
                    if (obj.Key == "fields")
                        continue;
                    realInfo.secucode = obj.Key;
                    //data_timestamp,shares_per_hand,last_px,business_amount,business_balance,open_px,high_px, low_px,preclose_px,offer_grp,bid_grp
                    JToken[] jTokenArray = obj.Value.ToArray();
                    for (int i = 0; i < jTokenArray.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                DateTime today = DateTime.Today;
                                int hhmmss = int.MaxValue;
                                if (int.TryParse(jTokenArray[i].ToString(), out hhmmss))
                                {
                                    int hour = hhmmss / 10000000;
                                    int minute = (hhmmss % 10000000) / 100000;
                                    int second = (hhmmss % 100000) / 1000;
                                    hour = hour < 24 ? hour : 23;
                                    minute = minute < 60 ? minute : 59;
                                    second = second < 60 ? second : 59;
                                    today = new DateTime(today.Year, today.Month, today.Day, hour, minute, second);
                                }
                                realInfo.data_timestamp = today.ToString("HH:mm:ss");
                                break;
                            case 1:
                                int sharesPerHand = int.MaxValue;
                                if (int.TryParse(jTokenArray[i].ToString(), out sharesPerHand))
                                    realInfo.shares_per_hand = sharesPerHand;
                                break;
                            case 2:
                                double preClosePrice = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out preClosePrice))
                                    realInfo.preclose_px = preClosePrice;
                                break;
                            case 3:
                                double lastPrice = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out lastPrice))
                                    realInfo.last_px = lastPrice;
                                break;
                            case 4:
                                double businessAmount = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out businessAmount))
                                    realInfo.business_amount = businessAmount;
                                break;
                            case 5:
                                double businessBalance = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out businessBalance))
                                    realInfo.business_balance = businessBalance;
                                break;
                            case 6:
                                double openPrice = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out openPrice))
                                    realInfo.open_px = openPrice;
                                break;
                            case 7:
                                double highPrice = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out highPrice))
                                    realInfo.high_px = highPrice;
                                break;
                            case 8:
                                double lowPrice = double.MaxValue;
                                if (double.TryParse(jTokenArray[i].ToString(), out lowPrice))
                                    realInfo.low_px = lowPrice;
                                break;
                            case 9:
                                realInfo.offer_grp = jTokenArray[i].ToString();
                                break;
                            case 10:
                                realInfo.bid_grp = jTokenArray[i].ToString();
                                break;
                        }
                    }
                    if (sinleBaseDataObj.dicSecuInfo.ContainsKey(realInfo.secucode))
                        realInfo.secuabbr = sinleBaseDataObj.dicSecuInfo[realInfo.secucode];
                    if (realInfo.preclose_px != 0d && realInfo.last_px != 0d)
                    {
                        realInfo.pct_change = realInfo.last_px - realInfo.preclose_px;
                        realInfo.pct_change_rate = 100 * (realInfo.last_px - realInfo.preclose_px) / realInfo.preclose_px;
                    }
                    else
                    {
                        realInfo.pct_change = 0d;
                        realInfo.pct_change_rate = 0d;
                    }
                    if (realInfo.last_px < realInfo.preclose_px)
                        realInfo.last_px = -realInfo.last_px;
                    if (realInfo.open_px < realInfo.preclose_px)
                        realInfo.open_px = -realInfo.open_px;
                    if (realInfo.high_px < realInfo.preclose_px)
                        realInfo.high_px = -realInfo.high_px;
                    if (realInfo.low_px < realInfo.preclose_px)
                        realInfo.low_px = -realInfo.low_px;

                    realDetail.lstRealInfo.Add(realInfo);
                }
            }
            catch (Exception ex)
            {
                m_log.Error(ex.ToString());
                return realDetail;
            }
            return realDetail;
        }

        public static void RefreshRealInfos(List<RealInfo> lstRealInfo)
        {
            for (int i = 0; i < lstRealInfo.Count; i++)
            {
                lstRealInfo[i] = (RealInfo)m_SecuCodeRealInfo[lstRealInfo[i].secucode];
            }
        }
        public static List<RealInfo> InitializeSearchRealInfo(string searchKey)
        {
            WizardDetail wizardDetail = InitializeWizardInfo(searchKey);
            List<RealInfo> lstRealInfo = new List<RealInfo>();
            foreach (WizardInfo wizardInfo in wizardDetail.data)
            {
                if (m_SecuCodeRealInfo.ContainsKey(wizardInfo.prod_code))
                    lstRealInfo.Add((RealInfo)m_SecuCodeRealInfo[wizardInfo.prod_code]);
            }
            lstRealInfo = lstRealInfo.OrderBy(item => item.secucode).ToList();
            return lstRealInfo;
        }
        public static List<RealInfo> InitializeRealInfo(string finance_mic)
        {
            List<RealInfo> lstRealInfo = new List<RealInfo>();
            Hashtable Category_RealDetail = (Hashtable)m_MarketRealDetail[finance_mic];
            foreach (var key in Category_RealDetail.Keys)
            {
                BaseDataObject baseDataObj = (BaseDataObject)Category_RealDetail[key];
                foreach (RealInfo realInfo in baseDataObj.m_realInfo.Values)
                {
                    lstRealInfo.Add(realInfo);
                }
            }
            lstRealInfo = lstRealInfo.OrderBy(item => item.secucode).ToList();
            return lstRealInfo;
        }
        public static List<RealInfo> InitializeRealInfo(string finance_mic, string category)
        {
            List<RealInfo> lstRealInfo = new List<RealInfo>();
            Hashtable Category_RealDetail = (Hashtable)m_MarketRealDetail[finance_mic];
            BaseDataObject baseDataObj = (BaseDataObject)Category_RealDetail[category];
            foreach (RealInfo realInfo in baseDataObj.m_realInfo.Values)
            {
                lstRealInfo.Add(realInfo);
            }
            lstRealInfo = lstRealInfo.OrderBy(item => item.secucode).ToList();
            return lstRealInfo;
        }
        public static WizardDetail InitializeWizardInfo(string searchKey)
        {
            return InitializeWizardDetailEventHandler(searchKey);
        }
    }
}
