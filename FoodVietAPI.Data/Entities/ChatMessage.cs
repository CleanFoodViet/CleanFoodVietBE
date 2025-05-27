using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ChatMessage
    {
        [Key]
        public Ulid ChatMessageId { get; set; }
        public Ulid SenderId { get; set; }
        public Ulid ReceiverId { get; set; }
        public string? MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public string MessageStatus { get; set; } = null!;

        public virtual Account Sender { get; set; } = null!;
        public virtual Account Receiver { get; set; } = null!;
    }
}
