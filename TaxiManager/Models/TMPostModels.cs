using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiManager.Models
{
    public class TMRequestPM
    {
        public int Id { get; set; }
        public string Start { get; set; }
        public string Destination { get; set; }
        public int Status { get; set; }
        public string DriverId { get; set; }
        public bool Reject { get; set; }
    }
    public class TMUserPM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PersonalName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}