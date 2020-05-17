using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _0131MVCTest3.Models
{
    public class City
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int ConfirmedNum { get; set; }
        public int Death { get; set; }
        public int Recovery { get; set; }

        public int UID { get; set; }
    }
}
