using System.ComponentModel.DataAnnotations;

namespace BlogAPI_EFCore.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
