using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class SchoolPrograms
	{
        public long Id { get; set; }
        public long School { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? Capacity { get; set; }
    }
}
