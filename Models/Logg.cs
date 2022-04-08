using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIsameDomain.Models
{
    public class Logg
    {
        public string Idx { get; set; }
        public string client { get; set; }
        public string exception { get; set; }
        public string datetime { get; set; }
    }
}