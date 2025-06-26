using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs
{
    public record ChatMessageDTO
    {
        public Ulid SenderId { get; set; }
        public Ulid ReceiverId { get; set; }
        public string? MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public string MessageStatus { get; set; } = null!;
    }
}