using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class ForumMessageAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ForumMessageId { get; set; }
        public ForumMessage ForumMessage { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public String FileName { get; set; }
        [Required]
        public String FilePath { get; set; }
    }
}
