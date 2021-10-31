using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace project.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set;}
        public string Title{ get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]

        public User User { get; set; }
    }
}
