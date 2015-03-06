using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace IDC_WEB
{
    public class OAuthService
    {
        private static ILog m_log = LogManager.GetLogger(typeof(OAuthService));

        private static string authorizeUrl = "http://open.hs.net/oauth2/oauth2/authorize";
        private static string tokenUrl = "http://open.hs.net/oauth2/oauth2/token";
        //private static string appKey = "263cf74f-344c-4553-bd3e-e42332ccb0a3";
        //private static string appSecret = "dce093f3-b0a9-48f2-bafe-acd7bac2ca7d";
        //private static string userName = "CarlSnow";
        //private static string password = "Carl2015";

        private static string appKey = "97b482de-671b-426b-8371-25a3b6293951";
        private static string appSecret = "6970cc39-93d0-44e6-b9e9-1982764d89ba";
        private static string userName = "Diore";
        private static string password = "810814";

        private static string defaultState = "test";
        private static string defaultModel = "code";
        private static string defaultScope = "info";
        private static string defaultClientType = "PC";
        private static string redirect_uri = "http://10.1.5.114:1213/";
        private static string code = "4244DEAD11D54EB2AC2F05B46149E5EF2015030610005178486627";//授权码模式-获得授权（返回Code）

        /// <summary>
        /// 授权码模式-获得授权（Step1）注：response_type必须为code
        /// </summary>
        public static string LicenseCodeModel_Authorize()
        {
            //Request：http://open.hundsun.com/oauth2/oauth2/authorize?response_type=code&client_id=263cf74f-344c-4553-bd3e-e42332ccb0a3&redirect_uri=http://10.1.5.114:1213/
            //Response：http://10.1.5.114:1213/?code=4244DEAD11D54EB2AC2F05B46149E5EF2015030610005178486627
            return string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}", authorizeUrl, appKey, redirect_uri);
        }
        /// <summary>
        /// 授权码模式-获得令牌（Step2）注：grant_type必须为authorization_code
        /// </summary>
        /// <returns></returns>
        public static OAuthInfo AuthorizationCodeModel()
        {
            OAuthInfo oAuthInfo = new OAuthInfo();
            try
            {
                string param = string.Format("grant_type=authorization_code&code={0}&redirect_url={1}&client_id={2}", code, redirect_uri, appKey);
                return InitializeOAuthInfo(param);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return oAuthInfo;
        }
        /// <summary>
        /// 隐式模式-获得授权 注：response_type必须为token（需要引导客户到服务商输入验证信息）
        /// </summary>
        public static string ImplicitModel_Authorize()
        {
            //Request：http://open.hundsun.com/oauth2/oauth2/authorize?response_type=token&client_id=263cf74f-344c-4553-bd3e-e42332ccb0a3&redirect_uri=http://10.1.5.114:1213/
            //Response：http://10.1.5.114:1213/#access_token=3A05F3B328D8438091E421B53D723D1C2015030609560190382729&expires_in=1425614161903&token_type=bearer&scope=trade%2Ciuc%2Cinfo
            string url = string.Format("{0}?response_type=token&client_id={1}&redirect_uri={2}", authorizeUrl, appKey, redirect_uri);
            return url;
        }
        /// <summary>
        /// 密码凭证模式  注：grant_type必须为password
        /// </summary>
        public static OAuthInfo PasswordModel()
        {
            OAuthInfo oAuthInfo = new OAuthInfo();
            try
            {
                string param = string.Format("grant_type=password&client_id={0}&redirect_uri={1}&client_secret={2}&username={3}&password={4}", appKey, redirect_uri, appSecret, userName, password);
                return InitializeOAuthInfo(param);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return oAuthInfo;
        }

        /// <summary>
        /// 客户端凭证模式  注：grant_type必须为client_credentials
        /// </summary>
        public static OAuthInfo ClientModel()
        {
            OAuthInfo oAuthInfo = new OAuthInfo();
            try
            {
                string param = string.Format("grant_type=client_credentials&client_id={0}&redirect_uri={1}&client_secret={2}", appKey, redirect_uri, appSecret);
                return InitializeOAuthInfo(param);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return oAuthInfo;
        }
        /// <summary>
        /// 刷新模式  注：grant_type必须为refresh_token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static OAuthInfo RefreshModel(string refresh_token)
        {
            OAuthInfo oAuthInfo = new OAuthInfo();
            try
            {
                string param = string.Format("grant_type=refresh_token&client_id={0}&redirect_uri={1}&client_secret={2}&refresh_token={3}", appKey, redirect_uri, appSecret, refresh_token);
                return InitializeOAuthInfo(param);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return oAuthInfo;
        }
        /// <summary>
        /// 初始化验证信息（OAuthInfo）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static OAuthInfo InitializeOAuthInfo(string param)
        {
            string str = appKey + ":" + appSecret;
            string key = Base64Helper.StringToBase64String(str);
            key = "Basic " + key;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tokenUrl);
            request.Method = "POST";
            request.Headers["Authorization"] = key;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] data = Encoding.UTF8.GetBytes(param);
            using (Stream streamR = request.GetRequestStream())
            {
                streamR.Write(data, 0, data.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            Char[] sReaderBuffer = new Char[256];
            int count = sReader.Read(sReaderBuffer, 0, 256);
            StringBuilder responseJson = new StringBuilder();
            while (count > 0)
            {
                String tempStr = new String(sReaderBuffer, 0, count);
                responseJson.Append(tempStr);
                count = sReader.Read(sReaderBuffer, 0, 256);
            }
            OAuthInfo oAuthInfo = (OAuthInfo)JsonConvert.DeserializeObject(responseJson.ToString(), typeof(OAuthInfo));
            return oAuthInfo;
        }
    }
}
