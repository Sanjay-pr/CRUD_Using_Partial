using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1_New.Models
{
    public class ParentInfo
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Contact { get; set; }
        public int StudentId {  get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student? student { get; set; }
    }
}
