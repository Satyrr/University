using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class VisitListWrapper
    {
        public int PatientID;
        public List<Visit> Visits { get; set; } = new List<Visit>();
        public Object Links
        {
            get
            {
                return new List<LinkInfo>()
                    {
                        new LinkInfo()
                        {
                            Href = "/Patients/" + this.PatientID + "/Visits/",
                            Rel = "create",
                            Method = "POST"
                        }
                    };
            }
        }
    }
}
