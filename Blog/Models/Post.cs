using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required(ErrorMessage = "Please, provide a content.")]
        [MaxLength(200, ErrorMessage = "200 characters or less.")]
        public string Content { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
