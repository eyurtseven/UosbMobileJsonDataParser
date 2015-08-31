using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UosbMobileJsonDataParser
{
    public class UOSBCompany
    {
        public string Id { get; set; }
        public string Company { get; set; }
        public string Sector { get; set; }
        public string Address { get; set; }
        public List<string> Phone { get; set; }
        public List<string> Fax { get; set; }
        public string WebSite { get; set; }
        public CompanyLocation Location { get; set; }
    }
}
