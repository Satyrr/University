using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        
        [JsonIgnore]
        public VisitListWrapper Visits { get; set; }
        public List<LinkInfo> Links {
            get
            {
                return new List<LinkInfo>()
                {
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.ID.ToString() + "/",
                        Rel = "self",
                        Method = "GET"
                    },
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.ID.ToString() + "/",
                        Rel = "edit",
                        Method = "PUT"
                    },
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.ID.ToString() + "/",
                        Rel = "remove",
                        Method = "DELETE"
                    },
                    new LinkInfo()
                    {
                        Href = "/Patients/" + this.ID.ToString() + "/Visits/",
                        Rel = "visits",
                        Method = "GET"
                    }
                };
            }
        }
    }
}
