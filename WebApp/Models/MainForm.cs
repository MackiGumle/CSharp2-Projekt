using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    // this is ASP NET Core MVC model for form data
    // implement validation
    public class MainForm
    {
        // Student
        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Jméno je povinné ")]
        public string FName { get; set; }

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Příjmení je povinné")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        public string Email { get; set; }

        [Display(Name = "Tel. číslo")]
        [Required(ErrorMessage = "Tel. číslo je povinné")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Neplatné telefonní číslo")]
        public string Phone { get; set; }


        // country should be text
        [Display(Name = "Stát")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Země je povinná")]
        public string Country { get; set; }

        [Display(Name = "Město")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Město je povinné")]
        public string City { get; set; }

        [Display(Name = "Ulice")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Ulice je povinná")]
        public string Street { get; set; }


        // Programs

        [Required(ErrorMessage = "Program je povinný")]
        public long Program1 { get; set; }

        public long? Program2 { get; set; }

        public long? Program3 { get; set; }

    }
}