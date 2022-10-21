using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        [MaxLength(50, ErrorMessage = "Name mustn't exceed 50 letters")]
        public string Name { get; set; }
        public int Age { get; set; }
        [RegularExpression("^0\\d{10}$", ErrorMessage = "Invalid phone number. ")]
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [NotMapped]
        [Compare("Email", ErrorMessage = "Email and Confirm Email dosen't match")]
        public string ConfirmEmail { get; set; }
        [MinLength(8, ErrorMessage = "Your Password is too short")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Password and Confirm Password dosen't match")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [DataType(DataType.Time)]
        public DateTime AttendanceTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime LeavingTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }
        [DisplayName("Image")]
        [ValidateNever]
        public string ImageUrl { get; set; }


    }
}
