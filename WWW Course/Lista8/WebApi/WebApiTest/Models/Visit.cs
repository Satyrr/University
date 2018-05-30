using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class Visit
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public DateTime VisitDateTime { get; set; }
     
        public Payment PaymentStatus { get; set; }

        public List<LinkInfo> Links
        {
            get
            {
                return new List<LinkInfo>()
                {
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.PatientID + "/" + "Visits/" + this.ID,
                        Rel = "self",
                        Method = "GET"
                    },
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.PatientID + "/" + "Visits/" + this.ID,
                        Rel = "edit",
                        Method = "PUT"
                    },
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.PatientID + "/" + "Visits/" + this.ID,
                        Rel = "remove",
                        Method = "DELETE"
                    }
                };
            }
        }
    }
}
