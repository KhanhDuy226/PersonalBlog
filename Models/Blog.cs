using System;
using System.ComponentModel.DataAnnotations;

namespace myBlog.Models
{
    public class Blog
    {
        [Key]
        public int ID { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "BlogTitle cannot be blank")]
        public string Title { get; set; }
        public string ImageURL { get; set; }

        public string Content { get; set; }
        public int Views { get; set; } = 0;
    }
}