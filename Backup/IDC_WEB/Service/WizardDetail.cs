using System.Collections.Generic;

namespace IDC_WEB
{
    public class WizardDetail
    {
        public List<WizardInfo> data;
        public List<string> autocomplete = new List<string>();
    }
    public class WizardInfo
    {
        public string prod_code;
        public string prod_name;
    }
}