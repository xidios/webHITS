using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class ForumCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public String Name { get; set; }
        public List<Forum> Forums { get; set; }
    }
}
