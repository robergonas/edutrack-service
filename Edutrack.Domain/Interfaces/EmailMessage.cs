using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Interfaces
{
    public class EmailMessage
    {
        public string[] To { get; set; } = [];
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<EmailAttachment>? Attachments { get; set; }
    }
}
