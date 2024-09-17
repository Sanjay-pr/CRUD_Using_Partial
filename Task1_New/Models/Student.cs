using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task1_New.Migrations;

namespace Task1_New.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public string Name { get; set; }
        public int? Age{ get; set; }
        public int? Standard { get; set; }

        //public ICollection<ParentInfo> parents { get; set; }
    }
}
