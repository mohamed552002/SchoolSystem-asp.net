using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SchoolSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class Student : Person
    {
        [Range(0,100,ErrorMessage ="Average mark is out of range")]
        public double AverageMark { get; set; }

        public int ClassId { get; set; }
        public string Grade { get; set; }
        [ValidateNever]
        public Class Class { get; set; }
    }
}
