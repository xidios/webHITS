using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class Forum
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public String Name { get; set; }
        [Required]
        public String Description { get; set; }

        public Guid ForumCategoryId { get; set; }
        public ForumCategory ForumCategory { get; set; }
        public List<ForumTopic> ForumTopics { get; set; }
    }
}
