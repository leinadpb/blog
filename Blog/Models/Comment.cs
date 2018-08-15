using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "500 characters or less")]
        public string Content { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateAt { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
