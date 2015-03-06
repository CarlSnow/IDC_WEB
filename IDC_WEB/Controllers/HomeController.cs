using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace IDC_WEB.Controllers
{
    public class HomeController : Controller
    {
        private static ILog m_log = LogManager.GetLogger(typeof(HomeController));
        public ActionResult OAuth()
        {
            ViewBag.Url = OAuthService.LicenseCodeModel_Authorize();
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult InitializeWizardInfo()
        {
            string responseStr = string.Empty;
            try
            {
                string searchKey = Request["searchKey"];
                WizardDetail wizardDetail = HttpService.InitializeWizardInfo(searchKey);
                responseStr = JsonConvert.SerializeObject(wizardDetail.data);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return Json(responseStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InitializeRealInfoByWizardInfo()
        {
            try
            {
                string searchKey = Request["searchKey"];
                searchKey = searchKey.Split('.')[0];
                Session["RealInfo"] = HttpService.InitializeSearchRealInfo(searchKey);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult InitializeBaseRealInfo()
        {
            try
            {
                string id = Request["id"];
                string[] idArray = id.Split('_');
                string finance_mic = idArray[0];
                string category = string.Empty;
                if (idArray.Length > 1)
                    category = idArray[1];
                if (category != string.Empty)
                    Session["RealInfo"] = HttpService.InitializeRealInfo(finance_mic, category);
                else
                    Session["RealInfo"] = HttpService.InitializeRealInfo(finance_mic);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult InitializePaginationInfo()
        {
            string responseStr = string.Empty;
            try
            {
                List<RealInfo> lstRealInfo = (List<RealInfo>)Session["RealInfo"];
                if (lstRealInfo == null) lstRealInfo = new List<RealInfo>();
                HttpService.RefreshRealInfos(lstRealInfo);
                int pagIndex = int.MaxValue;
                int.TryParse(Request["page"], out pagIndex);
                int limitCount = int.MaxValue;
                int.TryParse(Request["limit"], out limitCount);
                PaginationObject paginationObject = new PaginationObject();
                paginationObject.totalCount = lstRealInfo.Count;
                paginationObject.items = lstRealInfo.Skip((pagIndex - 1) * limitCount).Take(limitCount).ToList();
                responseStr = JsonConvert.SerializeObject(paginationObject);
            }
            catch (Exception ex)
            {
                m_log.Error(ex.Message);
            }
            return Json(responseStr, JsonRequestBehavior.AllowGet);
        }
    }
}
