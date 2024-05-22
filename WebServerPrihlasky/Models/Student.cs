using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
	public class Student
	{
		public long Id { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
	}
}
