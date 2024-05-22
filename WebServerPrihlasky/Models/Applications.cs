using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class Applications
	{
        public long Id { get; set; }
        public long Student { get; set; }
        public string Date { get; set; }
        public long Program1 { get; set; }
        public long? Program2 { get; set; }
        public long? Program3 { get; set; }
	}
}
