using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class Payment
    {
        public int PatientID { get; set; }
        public int VisitID { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public List<LinkInfo> Links
        {
            get
            {
                return new List<LinkInfo>()
                    {
                        new LinkInfo()
                        {
                            Href = "/Patients/" + this.PatientID + "/Visits/" + this.VisitID + "/PaymentStatus/",
                            Rel = "update",
                            Method = "PUT"
                        }
                    };
            }
        }
    }
}
