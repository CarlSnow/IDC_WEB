using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDC_WEB
{
    public class BaseDataObject
    {
        public List<string> lstSecuCode = new List<string>();
        public Hashtable m_realInfo = new Hashtable();
        public Dictionary<string, string> dicSecuInfo = new Dictionary<string, string>();
    }

    public class SingleBaseDataObject
    {
        public string en_prod_code = string.Empty;
        public Dictionary<string, string> dicSecuInfo = new Dictionary<string, string>();
        public string realJson = string.Empty;
    }
}