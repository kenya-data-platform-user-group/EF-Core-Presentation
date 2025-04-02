using System.ComponentModel.DataAnnotations;

namespace BlogAPI_EFCore.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string[] Tags { get; set; }

        //Reference to Author
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }


}
