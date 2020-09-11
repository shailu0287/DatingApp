using System;
using System.Collections.Generic;

namespace DatingApp.Data.Models
{
    public partial class Messages
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public int? RecipientId { get; set; }
        public string Content { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        public virtual User Recipient { get; set; }
        public virtual User Sender { get; set; }
    }
    [Serializable]
    public class QueueMessages
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public int? RecipientId { get; set; }
        public string Content { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
