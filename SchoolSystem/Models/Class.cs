using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        [Required(ErrorMessage = "Class name required")]
        [MinLength(1,ErrorMessage = "Class name mustn't be less than 1 letter")]
        [MaxLength(2,ErrorMessage = "Class name mustn't exceed 2 letters")]
        [DisplayName("Class Name")]
        public string Name { get; set; }
        [MinLength(3, ErrorMessage = "Class name mustn't be less than 3 letter")]
        [MaxLength(50, ErrorMessage = "Class name mustn't exceed 50 letters")]
        public string SuperVisor { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
